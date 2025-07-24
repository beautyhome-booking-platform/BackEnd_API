using Application.Voucher.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Voucher.Commands
{
    public class GetVoucherCommand : IRequest<GetVoucherResponse>
    {
        public Guid? VoucherId { get; set; }
        public string? Name { get; set; }
        public string? CreatedId { get; set; }
        public DateTime? Date { get; set; }
        public string? Code { get; set; }
        public bool? StillValid { get; set; }
        public Guid? ServiceOption { get; set;}
    }
}
