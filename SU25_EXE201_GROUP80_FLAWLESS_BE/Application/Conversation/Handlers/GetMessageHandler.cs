using Application.Conversation.Commands;
using Application.Conversation.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.Handlers
{
    public class GetMessageHandler : IRequestHandler<GetMessageCommand, GetMessageResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMessageHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetMessageResponse> Handle(GetMessageCommand request, CancellationToken cancellationToken)
        {
            var response = new GetMessageResponse();

            // Lấy tổng số tin nhắn
            var totalCount = (await _unitOfWork.MessageRepository.FindAsync(m => m.ConversationId == request.ConversationId)).Count();

            // Lấy tin nhắn với phân trang, mới nhất ở cuối
            var messages = await _unitOfWork.MessageRepository.FindAsync(
                m => m.ConversationId == request.ConversationId,
                q => q.OrderBy(m => m.SentAt)
                      .Skip((request.Page - 1) * request.PageSize)
                      .Take(request.PageSize));

            response.Messages = messages.Select(m => new MessageDTo
            {
                MessageId = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SentAt = m.SentAt,
                IsRead = m.IsRead,
                Status = m.Status,
            }).ToList();

            response.TotalCount = totalCount;
            response.IsSuccess = true;
            return response;
        }
    }
}
