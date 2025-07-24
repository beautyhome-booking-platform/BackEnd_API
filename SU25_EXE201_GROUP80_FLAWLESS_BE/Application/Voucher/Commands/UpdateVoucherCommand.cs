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
    public class UpdateVoucherCommand : IRequest<UpdateVoucherResponse>
    {
        [Required]
        public Guid Id { get; set; }
        public string? Code { get; set; } 
        public string? Description { get; set; }
        public DiscountType? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; } 
        public DiscountStyle? DiscountStype { get; set; } 
        public int? MinQuantity { get; set; }  
        public decimal? MinTotalAmount { get; set; } 
        public VoucherType? VoucherType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MaxUsage { get; set; }           
        public int? CurrentUsage { get; set; } = 0;  
        public Guid? ServiceOptionId { get; set; }
    }
}
