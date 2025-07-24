using Application.Voucher.Commands;
using Application.Voucher.Responses;
using Domain.DTO;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Voucher.Handlers
{
    public class GetVoucherHandler : IRequestHandler<GetVoucherCommand, GetVoucherResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetVoucherHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetVoucherResponse> Handle(GetVoucherCommand request, CancellationToken cancellationToken)
        {
            var response = new GetVoucherResponse();

            var query = await _unitOfWork.VoucherRepository.FindAsync(v =>
                (!request.VoucherId.HasValue || v.Id == request.VoucherId) &&
                (string.IsNullOrEmpty(request.Name) || v.Description.Contains(request.Name)) &&
                (string.IsNullOrEmpty(request.Code) || v.Code == request.Code) &&
                (string.IsNullOrEmpty(request.CreatedId) || v.CreatordId == request.CreatedId) &&
                (!request.Date.HasValue || (v.StartDate <= request.Date && v.EndDate >= request.Date)) &&
                (request.ServiceOption == null ||v.ServiceOption == request.ServiceOption)&&
                (!request.StillValid.HasValue ||
                    (request.StillValid.Value
                    ? (v.StartDate <= DateTime.UtcNow &&
                       v.EndDate >= DateTime.UtcNow &&
                       v.IsActive &&
                       (!v.MaxUsage.HasValue || v.CurrentUsage < v.MaxUsage))
                    : (v.EndDate < DateTime.UtcNow || !v.IsActive ||
                       (v.MaxUsage.HasValue && v.CurrentUsage >= v.MaxUsage))))
);

            var result = query.Select(v => new VoucherDTO
            {
                Id = v.Id,
                Code = v.Code,
                Description = v.Description,
                DiscountType = v.DiscountType,
                DiscountValue = v.DiscountValue,
                DiscountStype = v.DiscountStype.Value,
                MinQuantity = v.MinQuantity,
                MinTotalAmount = v.MinTotalAmount,
                StartDate = v.StartDate,
                EndDate = v.EndDate,
                MaxUsage = v.MaxUsage,
                CurrentUsage = v.CurrentUsage,
                CreatordId = v.CreatordId,
                ServiceOption = v.ServiceOption
            }).ToList();
            
            response.VoucherDTOs = result;
            response.IsSuccess = true;

            return response;
        }
    }
}
