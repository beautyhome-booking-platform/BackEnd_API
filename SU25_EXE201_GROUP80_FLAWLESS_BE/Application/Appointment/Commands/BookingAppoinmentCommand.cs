using Application.Appointment.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Commands
{
    public class BookingAppoinmentCommand : IRequest<BookingAppoinmentResponse>
    {
        public string CustomerId { get; set; }
        public string ArtistMakeupId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime StartTime { get; set; } // Thời gian bắt đầu của cuộc hẹn
        public DateTime EndTime { get; set; } // Thời gian kết thúc của cuộc hẹn
        public string Address { get; set; }
        public string? Note { get; set; }
        public List<AppointmentDetailRequest> AppointmentDetails { get; set; } = new List<AppointmentDetailRequest>();
        public Guid? VoucherId { get; set; }
    }
    public class AppointmentDetailRequest
    {
        public Guid ServiceOptionId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Note { get; set; }
    }
}
