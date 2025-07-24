using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? ImageUrlCustomer { get; set; }
        public string ArtistMakeupId { get; set; }
        public string ArtistMakeupName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Address { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; }
        public Guid? VoucherId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAmountAfterDiscount { get; set; }
        public decimal DepositForApp { get; set; }
        public decimal AmountToPayForArtist { get; set; }
        public List<AppointmentDetailDTO> AppointmentDetails { get; set; }
    }
    public class AppointmentDetailDTO
    {
        public Guid Id { get; set; }
        public Guid ServiceOptionId { get; set; }
        public string ServiceOptionName { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
