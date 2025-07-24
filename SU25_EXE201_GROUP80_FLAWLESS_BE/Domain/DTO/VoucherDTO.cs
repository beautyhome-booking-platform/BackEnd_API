using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class VoucherDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }   // Mã voucher

        public string? Description { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }
        [Required]
        public decimal DiscountValue { get; set; } // Kiểu giảm (0=Phần trăm,1=Tiền)
        [Required]
        public DiscountStyle DiscountStype { get; set; } // Kiểu áp dụng giảm (0=theo số lượng, 1=theo tổng tiền)
        public int? MinQuantity { get; set; }  // Số lượng tối thiểu để áp dụng giảm giá (nếu DiscountStype=0)
        public decimal? MinTotalAmount { get; set; } // Tổng tiền tối thiểu để áp dụng giảm giá (nếu DiscountStype=1)

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int? MaxUsage { get; set; }            // Giới hạn số lần dùng
        public int? CurrentUsage { get; set; } = 0;   // Đếm số lần đã dùng

        public string CreatordId { get; set; }          // Người tạo
        public Guid? ServiceOption { get; set; }
    }
}
