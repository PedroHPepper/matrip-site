using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma17SubTripValueRepository : BaseRepository<ma17SubtripValue>, Ima17SubTripValueRepository
    {
        public ma17SubTripValueRepository(ApplicationDbContext _DbContext) : base(_DbContext)
        {
        }

        public ma17SubtripValue GetSubTripValue(int SubTripID)
        {
            var query = _DbContext.ma17SubtripValue.Where(e => e.FK1714idsubtrip == SubTripID);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
    }
}
