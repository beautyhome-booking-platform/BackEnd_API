﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.IRepository
{
    public interface IChatBoxAiRepository : IGenericRepository<ChatHistory>
    {
    }
}
