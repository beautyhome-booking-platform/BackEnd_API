using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class BankAccountDTO
    {
        public Guid Id { get; set; }
        public string Bank { get; set; }                    // Ví dụ: Vietcombank
        public string Stk { get; set; }                     // Số tài khoản
        public string Name { get; set; }                    // Tên chủ tài khoản
    }
}
