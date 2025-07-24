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

    public class UpdateVoucherHandler : IRequestHandler<UpdateVoucherCommand, UpdateVoucherResponse>
    {
        IUnitOfWork _unitOfWork;
        public UpdateVoucherHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UpdateVoucherResponse> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateVoucherResponse();

            // Tìm voucher theo Code (nếu unique) hoặc Id (bạn nên cân nhắc thêm Id nếu cần)
            var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(request.Id);

            if (voucher == null || voucher.IsDeleted)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Voucher không tồn tại!";
                return response;
            }

            // Cập nhật các trường nếu truyền lên
            if (!string.IsNullOrEmpty(request.Description))
                voucher.Description = request.Description;
            if (request.DiscountType.HasValue)
                voucher.DiscountType = request.DiscountType.Value;
            if (request.DiscountValue.HasValue)
                voucher.DiscountValue = request.DiscountValue.Value;
            if (request.DiscountStype.HasValue)
                voucher.DiscountStype = request.DiscountStype.Value;
            if (request.MinQuantity.HasValue)
                voucher.MinQuantity = request.MinQuantity.Value;
            if (request.MinTotalAmount.HasValue)
                voucher.MinTotalAmount = request.MinTotalAmount.Value;
            if (request.VoucherType.HasValue)
                voucher.VoucherType = request.VoucherType.Value;
            if (request.StartDate.HasValue)
                voucher.StartDate = request.StartDate.Value;
            if (request.EndDate.HasValue)
                voucher.EndDate = request.EndDate.Value;
            if (request.MaxUsage.HasValue)
                voucher.MaxUsage = request.MaxUsage.Value;
            if (request.CurrentUsage.HasValue)
                voucher.CurrentUsage = request.CurrentUsage.Value;
            if (request.ServiceOptionId.HasValue)
                voucher.ServiceOption = request.ServiceOptionId.Value;

            // Update và save
            _unitOfWork.VoucherRepository.Update(voucher);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;

            return response;
        }
    }
}
