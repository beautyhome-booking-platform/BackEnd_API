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
    public class AddBackAccountHandler : IRequestHandler<AddBackAccountCommand, AddBackAccountResponse>
    {
        IUnitOfWork _unitOfWork;
        public AddBackAccountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AddBackAccountResponse> Handle(AddBackAccountCommand request, CancellationToken cancellationToken)
        {
            var response = new AddBackAccountResponse();

            var check = (await _unitOfWork.BankInfoRepository.FindAsync(bi => bi.AccountNumber == request.AccountNumber)).FirstOrDefault();
            if (check != null)
            {
                response.ErrorMessage = "This account number already exists.";
                return response;
            }

            var user = (await _unitOfWork.UserAppRepository.FindAsync(u => u.Id == request.UserId)).FirstOrDefault();
            if (user == null)
            {
                response.ErrorMessage = "User not found.";
                return response;
            }
            var bankInfo = new Domain.Entities.BankInfo
            {
                User = user,
                BankName = request.BankName,
                AccountNumber = request.AccountNumber,
                AccountHolder = request.AccountHolderName
            };

            _unitOfWork.BankInfoRepository.AddAsync(bankInfo);
            await _unitOfWork.SaveChangesAsync();
            response.IsSuccess = true;
            return response;
        }
    }
}
