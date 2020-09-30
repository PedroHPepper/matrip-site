using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma14SubTripRepository : BaseRepository<ma14subtrip>, Ima14SubTripRepository
    {
        public ma14SubTripRepository(ApplicationDbContext _DbContext) : base(_DbContext)
        {
        }

        public List<ma14subtrip> GetSubTripList(int TripID)
        {
            return _DbContext.ma14subtrip.Where(e => e.FK1405idtrip == TripID).ToList();
        }

        public List<ma14subtrip> GetSubTripGuideList(int GuideID)
        {
            var query = _DbContext.ma14subtrip.Include(e => e.ma05trip)
                .Include(e => e.ma12SubtripGuide)
                .Where(e => e.ma12SubtripGuide.Where(w => w.ma12idSubtripGuide == GuideID).ToList() != null);
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }


    }
}
