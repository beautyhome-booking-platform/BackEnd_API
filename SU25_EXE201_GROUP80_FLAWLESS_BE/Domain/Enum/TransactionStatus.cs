﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum TransactionStatus
    {
        Pending     = 0,
        Completed   = 1,
        Failed      = 2,
        Cancelled   = 3
    }
}
