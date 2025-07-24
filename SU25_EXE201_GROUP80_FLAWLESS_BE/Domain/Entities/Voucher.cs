using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entities
{
    public class Voucher : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }   // Mã voucher

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }
        [Required]
        public decimal DiscountValue { get; set; } 
        [Required]
        public DiscountStyle? DiscountStype { get; set; } // Kiểu áp dụng giảm (0=theo số lượng, 1=theo tổng tiền)
        public int? MinQuantity { get; set; }  // Số lượng tối thiểu để áp dụng giảm giá (nếu DiscountStype=0)
        public decimal? MinTotalAmount { get; set; } // Tổng tiền tối thiểu để áp dụng giảm giá (nếu DiscountStype=1)

        [Required]
        public VoucherType VoucherType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public int? MaxUsage { get; set; }            // Giới hạn số lần dùng
        public int? CurrentUsage { get; set; } = 0;   // Đếm số lần đã dùng

        public string CreatordId { get; set; }          // Người tạo

        public Guid? ServiceOption { get; set;}
    }
}
