using Application.BankInfo.Commands;
using Application.BankInfo.Responses;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BankInfo.Handlers
{
    public class GetBankAccountHandler : IRequestHandler<GetBankAccountCommand, GetBankAccountResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetBankAccountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetBankAccountResponse> Handle(GetBankAccountCommand request, CancellationToken cancellationToken)
        {
            var response = new GetBankAccountResponse();

            var bankInfos = await _unitOfWork.BankInfoRepository.FindAsync(bi =>
                (string.IsNullOrEmpty(request.AccountNumber) || bi.AccountNumber == request.AccountNumber) &&
                (request.Id == null || bi.Id == request.Id) &&
                (request.UserId == bi.User.Id || request.UserId == null)&&
                !bi.IsDeleted
            );

            var bankInfoList = bankInfos?.ToList() ?? new List<Domain.Entities.BankInfo>();

            if (!bankInfoList.Any())
            {
                response.ErrorMessage = "Bank account not found.";
                response.BankAccount = new List<BankAccountDTO>();
                return response;
            }

            response.BankAccount = bankInfoList.Select(bi => new BankAccountDTO
            {
                Id = bi.Id,
                Bank = bi.BankName,
                Stk = bi.AccountNumber,
                Name = bi.AccountHolder
            }).ToList();

            response.IsSuccess = true;
            return response;
        }
    }
}
