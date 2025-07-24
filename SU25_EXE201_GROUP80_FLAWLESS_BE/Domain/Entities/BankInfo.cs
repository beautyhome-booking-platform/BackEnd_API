using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BankInfo : BaseEntity
    {
        [Required]
        [ForeignKey("UserId")]
        public UserApp User { get; set; }

        public string BankName { get; set; }

        public string AccountNumber { get; set; }

        public string AccountHolder { get; set; }

    }
}
