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
    public class DeleteBankAccountHandler : IRequestHandler<DeleteBankAccountCommand, DeleteBankAccountResponse>
    {
        IUnitOfWork _unitOfWork;
        public DeleteBankAccountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteBankAccountResponse> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteBankAccountResponse();

            var bankAccount = await _unitOfWork.BankInfoRepository.GetByIdAsync(request.Id);
            if (bankAccount == null)
            {
                response.ErrorMessage = "Tài khoản ngân hàng không tồn tại";
                return response;
            }

            bankAccount.IsDeleted = true;
            _unitOfWork.BankInfoRepository.Update(bankAccount);
            await _unitOfWork.SaveChangesAsync();
            response.IsSuccess = true;

            return response;
        }
    }
}
