using Application.Transaction.Commands;
using Application.Transaction.Responses;
using Domain.DTO;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transaction.Handlers
{
    public class GetTransactionHandler : IRequestHandler<GetTransactionCommand, GetTransactionResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetTransactionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetTransactionResponse> Handle(GetTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new GetTransactionResponse();

            var transacions = await _unitOfWork.TransactionRepository.FindAsync(t => (t.Id == request.Id || request.Id == null) &&
                                                                                (t.AppointmentId == request.AppointmentId || request.AppointmentId == null)&&
                                                                                (t.UserId == request.UserId || t.ArtistId == request.UserId || string.IsNullOrEmpty(request.UserId)) &&
                                                                                (!t.IsDeleted)&&
                                                                                (!request.Status.HasValue || (int)t.TransactionStatus == (int)request.Status.Value)
                                                                                );
            response.TransactionDTOs = transacions
                                    .Select(x => new TransactionDTO
                                    {
                                        Id = x.Id,
                                        AppointmentId = (Guid)x.AppointmentId,
                                        Amount = x.Amount,
                                        TransactionType = x.TransactionType,
                                        PaymentProvider = x.PaymentProvider,
                                        CreatedAt = DateTime.UtcNow,
                                        PaymentProviderTxnId = x.PaymentProviderTxnId,
                                        TransactionStatus = x.TransactionStatus,
                                        // ...
                                    }).ToList();
            response.IsSuccess = true;
            return response;
        }
    }
}
