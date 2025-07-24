using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            IsSuccess = false;
            ErrorMessage = string.Empty;
        }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

    }
}
