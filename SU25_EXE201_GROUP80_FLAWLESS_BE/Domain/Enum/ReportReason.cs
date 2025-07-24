using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum ReportReason
    {
        Fraud                   = 0,              // Lừa đảo
        InappropriateBehavior   = 1,        // Hành vi không phù hợp
        FalseInformation        = 2,       // Thông tin sai sự thật
        PoorService             = 3,            // Dịch vụ kém
        Other                   = 4                   // Lý do khác
    }
}
