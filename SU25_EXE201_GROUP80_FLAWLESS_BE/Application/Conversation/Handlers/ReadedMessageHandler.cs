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
    public class ReadedMessageHandler : IRequestHandler<ReadedMessageCommand, ReadedMessageResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<ChatHub> _hubContext;
        public ReadedMessageHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IHubContext<ChatHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }
        public async Task<ReadedMessageResponse> Handle(ReadedMessageCommand request, CancellationToken cancellationToken)
        {
            var response = new ReadedMessageResponse();

            // Lấy userId từ JWT
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Unauthorized";
                return response;
            }

            // Tìm các message của user (là receiver)
            var messages = await _unitOfWork.MessageRepository.FindAsync(
                m => request.MessageIds.Contains(m.Id) && m.ReceiverId == userId);

            if (messages == null || !messages.Any())
            {
                response.IsSuccess = false;
                response.ErrorMessage = "No message to update";
                return response;
            }

            foreach (var msg in messages)
            {
                msg.IsRead = true;
                msg.ReadAt = DateTime.UtcNow;
                // Bạn có thể cập nhật Status nếu cần: msg.Status = MessageStatus.Read;
            }
            await _unitOfWork.SaveChangesAsync();

            // Phát realtime qua SignalR cho từng người gửi
            foreach (var msg in messages)
            {
                // Gửi cho người gửi để họ biết tin nhắn đã đọc
                await _hubContext.Clients.User(msg.SenderId)
                    .SendAsync("MessageRead", new
                    {
                        messageId = msg.Id,
                        receiverId = userId,
                        readAt = msg.ReadAt
                    });

                // Optionally gửi luôn cho receiver (nếu FE muốn update UI)
                await _hubContext.Clients.User(userId)
                    .SendAsync("MessageRead", new
                    {
                        messageId = msg.Id,
                        receiverId = userId,
                        readAt = msg.ReadAt
                    });
            }

            response.ReadMessageIds = messages.Select(m => m.Id).ToList();
            response.IsSuccess = true;
            return response;
        }   
    }
}
