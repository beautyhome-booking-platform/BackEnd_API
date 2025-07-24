using Application.BankInfo.Commands;
using Application.BankInfo.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BankInfo.Handlers
{
    public class UpdateBankAccountHandler : IRequestHandler<UpdateBankAccountCommand, UpdateBankAccountResponse>
    {
        IUnitOfWork _unitOfWork;
        public UpdateBankAccountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UpdateBankAccountResponse> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateBankAccountResponse();

            var bankAccount = await _unitOfWork.BankInfoRepository.GetByIdAsync(request.Id);
            if (bankAccount == null)
            {
                response.ErrorMessage = "Không tìm thấy tài khoản ngân hàng";
                return response;
            }

            if(!string.IsNullOrEmpty(request.BankName))
                bankAccount.BankName = request.BankName;
            if (!string.IsNullOrEmpty(request.AccountNumber))
                bankAccount.AccountNumber = request.AccountNumber;
            if (!string.IsNullOrEmpty(request.AccountHolderName))
                bankAccount.AccountHolder = request.AccountHolderName;

            _unitOfWork.BankInfoRepository.Update(bankAccount);
            await _unitOfWork.SaveChangesAsync();
            response.IsSuccess = true;
            return response;
        }
    }
}
