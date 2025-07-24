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
    public class UserAppRepository : GenericRepository<UserApp>, IUserAppRepository
    {
        public UserAppRepository(FlawlessDBContext context) : base(context)
        {
        }
    }
}
