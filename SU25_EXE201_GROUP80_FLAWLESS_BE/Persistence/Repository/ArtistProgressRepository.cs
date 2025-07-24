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
    public class ArtistProgressRepository : GenericRepository<ArtistProgress>, IArtistProgressRepository
    {
        public ArtistProgressRepository(FlawlessDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ArtistProgress>> FindAsync(Expression<Func<ArtistProgress, bool>> predicate)
        {
            return await _context.ArtistsProgresss
                .Include(ap => ap.Artist)
                .Where(predicate)
                .ToListAsync();
        }

        public IQueryable<ArtistProgress> GetQueryable()
        {
            return _context.Set<ArtistProgress>();
        }
    }
}
