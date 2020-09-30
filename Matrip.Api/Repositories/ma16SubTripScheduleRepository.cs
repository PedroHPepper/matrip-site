using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma16SubTripScheduleRepository : BaseRepository<ma16subtripschedule>, Ima16SubTripScheduleRepository
    {
        public ma16SubTripScheduleRepository(ApplicationDbContext _DbContext) : base(_DbContext)
        {
        }

        public ma16subtripschedule GetSubTripSchedule(int SubTripID)
        {
            var query = _DbContext.ma16subtripschedule.Where(e => e.FK1614idsubtrip == SubTripID);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
    }
}
