using Domain.Entities;
using Persistence.Data;
using Persistence.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class ChatBoxAiRepository : GenericRepository<ChatHistory>, IChatBoxAiRepository
    {
        public ChatBoxAiRepository(FlawlessDBContext context) : base(context)
        {
        }
    }
}
