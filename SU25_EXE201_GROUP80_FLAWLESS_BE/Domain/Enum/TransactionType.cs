using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum TransactionType
    {
        CustomerPayment             = 0,
        Refund                      = 1,
        CancellationPayoutArtist    = 2,       
    }
}
