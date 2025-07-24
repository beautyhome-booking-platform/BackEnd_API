using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ChatHistory : BaseEntity
    {
        public string UserId { get; set; }
        public string UserMessage { get; set; }
        public string AIReply { get; set; }
    }
}
