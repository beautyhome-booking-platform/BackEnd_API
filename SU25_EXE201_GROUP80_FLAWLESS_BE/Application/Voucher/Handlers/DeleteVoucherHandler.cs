using Application.Voucher.Commands;
using Application.Voucher.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Voucher.Handlers
{
    public class DeleteVoucherHandler : IRequestHandler<DeleteVoucherCommand, DeleteVoucherResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteVoucherHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteVoucherResponse> Handle(DeleteVoucherCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteVoucherResponse();

            var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(request.Id);
            if (voucher == null || voucher.IsDeleted)
            {
                response.ErrorMessage = "Voucher không tồn tại";
                return response;
            }

            voucher.IsDeleted = true;
            _unitOfWork.VoucherRepository.Update(voucher);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;

            return response;
        }
    }
}
