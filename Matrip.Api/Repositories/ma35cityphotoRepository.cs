using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma35cityphotoRepository : BaseRepository<ma35cityphoto>, Ima35cityphotoRepository
    {
        public ma35cityphotoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ma35cityphoto GetCityPhoto(int CityID)
        {
            var query = _DbContext.ma35cityphoto.Where(e => e.FK3509idcity == CityID);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
    }
}
