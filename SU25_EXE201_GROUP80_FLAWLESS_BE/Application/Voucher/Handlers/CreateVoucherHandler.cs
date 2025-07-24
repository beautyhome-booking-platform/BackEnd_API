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
    public class CreateVoucherHandler : IRequestHandler<CreateVoucherCommand, CreateVoucherResponse>
    {
        IUnitOfWork _unitOfWorkk;
        public CreateVoucherHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWorkk = unitOfWork;
        }

        public async Task<CreateVoucherResponse> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateVoucherResponse();

            // Kiểm tra trùng mã voucher
            var existingVoucher = await _unitOfWorkk.VoucherRepository.FindAsync(v => v.Code == request.Code);
            if (existingVoucher.Any())
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Mã voucher đã tồn tại.";
                return response;
            }

            // Validate điều kiện áp dụng theo DiscountStyle
            if (request.DiscountStype == Domain.Enum.DiscountStyle.Quantity && request.MinQuantity == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Vui lòng nhập MinQuantity khi áp dụng theo số lượng.";
                return response;
            }
            if (request.DiscountStype == Domain.Enum.DiscountStyle.TotalAmount && request.MinTotalAmount == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Vui lòng nhập MinTotalAmount khi áp dụng theo tổng tiền.";
                return response;
            }

            // Khởi tạo voucher
            var voucher = new Domain.Entities.Voucher
            {
                Code = request.Code,
                Description = request.Description,
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue,
                DiscountStype = request.DiscountStype,
                MinQuantity = request.MinQuantity,
                MinTotalAmount = request.MinTotalAmount,
                VoucherType = request.VoucherType,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true,
                MaxUsage = request.MaxUsage,
                CurrentUsage = request.CurrentUsage ?? 0,
                CreatordId = request.CreatordId,
                ServiceOption = request.ServiceOptionId
            };

            _unitOfWorkk.VoucherRepository.AddAsync(voucher);
            await _unitOfWorkk.SaveChangesAsync();

            response.IsSuccess = true;
            response.VoucherId = voucher.Id;

            return response;
        }
    }
}
