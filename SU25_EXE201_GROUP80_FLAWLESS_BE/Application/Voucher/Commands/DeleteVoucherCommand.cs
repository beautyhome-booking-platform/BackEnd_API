using Application.Voucher.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Voucher.Commands
{
    public class DeleteVoucherCommand : IRequest<DeleteVoucherResponse>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
