using Application.Voucher.Responses;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Voucher.Commands
{
    public class CreateVoucherCommand : IRequest<CreateVoucherResponse>
    {
        public string Code { get; set; }   // Mã voucher

        public string? Description { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }
        [Required]
        public decimal DiscountValue { get; set; } // Kiểu giảm (0=Phần trăm,1=Tiền)
        [Required]
        public DiscountStyle? DiscountStype { get; set; } // Kiểu áp dụng giảm (0=theo số lượng, 1=theo tổng tiền)
        public int? MinQuantity { get; set; }  // Số lượng tối thiểu để áp dụng giảm giá (nếu DiscountStype=0)
        public decimal? MinTotalAmount { get; set; } // Tổng tiền tối thiểu để áp dụng giảm giá (nếu DiscountStype=1)

        public VoucherType VoucherType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int? MaxUsage { get; set; }            // Giới hạn số lần dùng
        public int? CurrentUsage { get; set; } = 0;   // Đếm số lần đã dùng
        [Required]
        public string CreatordId { get; set; }          // Người tạo

        public Guid? ServiceOptionId { get; set; }
    }
}
