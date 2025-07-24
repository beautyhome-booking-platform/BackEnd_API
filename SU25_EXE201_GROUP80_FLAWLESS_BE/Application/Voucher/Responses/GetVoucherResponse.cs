using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Voucher.Responses
{
    public class GetVoucherResponse : BaseResponse
    {
        public List<VoucherDTO> VoucherDTOs { get; set; } = new List<VoucherDTO>();
    }
}
