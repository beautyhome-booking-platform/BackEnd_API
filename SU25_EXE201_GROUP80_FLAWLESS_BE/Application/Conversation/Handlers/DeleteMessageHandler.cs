using Application.Conversation.Commands;
using Application.Conversation.Responses;
using Domain.Enum;
using Infrastructure.ServiceHelp;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.Handlers
{
    public class DeleteMessageHandler : IRequestHandler<DeleteMessageCommand, DeleteMessageResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _hubContext;

        public DeleteMessageHandler(IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }
        public async Task<DeleteMessageResponse> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteMessageResponse();

            var message = (await _unitOfWork.MessageRepository.FindAsync(m => m.Id == request.MessageId)).FirstOrDefault();
            if (message == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Message not found";
                return response;
            }

            message.Status = MessageStatus.Deleted;
            await _unitOfWork.SaveChangesAsync();

            // Phát realtime event MessageDeleted cho cả sender và receiver
            await _hubContext.Clients.User(message.SenderId).SendAsync("MessageDeleted", new { messageId = message.Id });
            await _hubContext.Clients.User(message.ReceiverId).SendAsync("MessageDeleted", new { messageId = message.Id });

            response.IsSuccess = true;
            return response;
        }
    }
}
