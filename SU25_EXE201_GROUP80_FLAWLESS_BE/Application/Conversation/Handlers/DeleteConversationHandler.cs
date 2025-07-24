using Application.Conversation.Commands;
using Application.Conversation.Responses;
using Infrastructure.ServiceHelp;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.Handlers
{
    public class DeleteConversationHandler : IRequestHandler<DeleteConversationCommand, DeleteConversationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<ChatHub> _hubContext;

        public DeleteConversationHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IHubContext<ChatHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }
        public async Task<DeleteConversationResponse> Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteConversationResponse();

            // Lấy userId hiện tại
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Unauthorized";
                return response;
            }

            // Lấy conversation đúng của user
            var conversation = (await _unitOfWork.ConversationRepository.FindAsync(
                c => c.Id == request.ConversationId &&
                    (c.User1Id == userId || c.User2Id == userId))).FirstOrDefault();

            if (conversation == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Conversation not found or access denied";
                return response;
            }

            // Soft delete (cờ IsArchived), hoặc xóa hẳn tùy bạn
            conversation.IsArchived = true;
            await _unitOfWork.SaveChangesAsync();

            // Phát sự kiện SignalR cho cả hai phía để ẩn/conversation bị xóa trên UI
            await _hubContext.Clients.User(conversation.User1Id)
                .SendAsync("ConversationDeleted", new { conversationId = conversation.Id });
            await _hubContext.Clients.User(conversation.User2Id)
                .SendAsync("ConversationDeleted", new { conversationId = conversation.Id });

            response.IsSuccess = true;
            return response;
        }
    }
}
