using Application.Conversation.Commands;
using Application.Conversation.Responses;
using Domain.Entities;
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
    public class SendMessageHandler : IRequestHandler<SendMessageCommand, SendMessageResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendMessageHandler(IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<SendMessageResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var response = new SendMessageResponse();

            // Lấy senderId từ JWT
            var senderId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Unauthorized";
                return response;
            }

            // Tìm hoặc tạo Conversation giữa 2 user (không phân biệt chiều)
            var conversation = (await _unitOfWork.ConversationRepository.FindAsync(c =>
                (c.User1Id == senderId && c.User2Id == request.ReceiverId) ||
                (c.User2Id == senderId && c.User1Id == request.ReceiverId))).FirstOrDefault();

            if (conversation == null)
            {
                conversation = new Domain.Entities.Conversation
                {
                    User1Id = senderId,
                    User2Id = request.ReceiverId,
                    LastMessageTime = DateTime.UtcNow,
                    IsArchived = true,
                };
                _unitOfWork.ConversationRepository.AddAsync(conversation);
                await _unitOfWork.SaveChangesAsync();
            }

            // Tạo message
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = request.ReceiverId,
                Content = request.Content,
                ConversationId = conversation.Id,
                SentAt = DateTime.UtcNow,
                Status = Domain.Enum.MessageStatus.Sent,
                IsRead = false
            };
            _unitOfWork.MessageRepository.AddAsync(message);

            // Cập nhật thời gian tin nhắn cuối
            conversation.LastMessageTime = message.SentAt;
            await _unitOfWork.SaveChangesAsync();

            // Gửi SignalR cho người nhận và người gửi (để cập nhật chat realtime 2 bên)
            var msgDto = new MessageDto
            {
                MessageId = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                SentAt = message.SentAt,
                ConversationId = message.ConversationId,
                IsRead = message.IsRead
            };

            await _hubContext.Clients.User(request.ReceiverId)
                .SendAsync("ReceiveMessage", msgDto);

            await _hubContext.Clients.User(senderId)
                .SendAsync("ReceiveMessage", msgDto);

            
            response.IsSuccess = true;
            response.Message = msgDto;
            return response;
        }
    }
}
