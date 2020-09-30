using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma12SubtripGuideRepository : BaseRepository<ma12SubtripGuide>, Ima12SubtripGuideRepository
    {
        public ma12SubtripGuideRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<ma12SubtripGuide> GetBySubTripID(int SubtripID)
        {
            var query = _DbContext.ma12SubtripGuide.Where(e => e.FK1214idsubtrip == SubtripID);
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }
    }
}
