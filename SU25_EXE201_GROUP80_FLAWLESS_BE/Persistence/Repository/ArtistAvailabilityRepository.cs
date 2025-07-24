using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class ArtistAvailabilityRepository : GenericRepository<ArtistAvailability>, IArtistAvailabilityRepository
    {
        public ArtistAvailabilityRepository(FlawlessDBContext context) : base(context)
        {
        }

    }
}
