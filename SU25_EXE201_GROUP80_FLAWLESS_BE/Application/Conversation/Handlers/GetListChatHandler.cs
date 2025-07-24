using Application.Conversation.Commands;
using Application.Conversation.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.Handlers
{
    public class GetListChatHandler : IRequestHandler<GetListChatCommand, GetListChatResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetListChatHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetListChatResponse> Handle(GetListChatCommand request, CancellationToken cancellationToken)
        {
            var response = new GetListChatResponse();

            // Lấy userId từ JWT
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Unauthorized";
                return response;
            }

            // Lấy các conversation có tham gia
            var conversations = await _unitOfWork.ConversationRepository.FindAsync(
                c => c.User1Id == userId || c.User2Id == userId,
                q => q
                    .Include(x => x.User1)
                    .Include(x => x.User2)
                    .Include(x => x.Messages.OrderByDescending(m => m.SentAt))
                    .OrderByDescending(c => c.LastMessageTime)
);

            var conversationList = conversations.Select(conv =>
            {
                // Xác định partner là ai
                var isUser1 = conv.User1Id == userId;
                var partner = isUser1 ? conv.User2 : conv.User1;

                // Lấy tin nhắn cuối cùng
                var lastMsg = conv.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();

                return new ChatConversationDto
                {
                    ConversationId = conv.Id,
                    PartnerId = partner?.Id,
                    PartnerName = partner?.Name,
                    PartnerAvatar = partner?.ImageUrl,
                    LastMessage = lastMsg?.Content,
                    LastMessageTime = lastMsg?.SentAt,
                    IsArchived = conv.IsArchived
                };
            }).ToList();

            response.Conversations = conversationList;
            response.IsSuccess = true;
            return response;
        }
    }
}
