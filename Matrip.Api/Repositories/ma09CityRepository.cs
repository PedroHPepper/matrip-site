using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma09CityRepository : BaseRepository<ma09city>, Ima09CityRepository
    {
        public ma09CityRepository(ApplicationDbContext DbContext) : base(DbContext)
        {
        }
        public ma09city GetByName(int UFid, string City)
        {
            var query = _DbContext.Set<ma09city>().Where<ma09city>(e => e.FK0908iduf == UFid && e.ma09name == City)
                .Include(e => e.ma35cityphoto);

            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
        public List<ma09city> GetCities()
        {
            var query = _DbContext.ma09city.AsNoTracking()
                .Include(e => e.ma08uf).Where(e => e.ma05trip.Any());
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }
        public ma09city GetCityByID(int CityID)
        {
            var query = _DbContext.ma09city.Where(e => e.ma09idcity == CityID)
                .Include(e => e.ma35cityphoto);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
        public List<ma09city> GetSearch(string cityText, string UF)
        {
            if(!string.IsNullOrEmpty(UF))
            {
                var query = _DbContext.ma09city.Include(e => e.ma08uf)
                .Where(e => e.ma08uf.ma08UFInitials == UF && TextManipulation.RemoveAccents(e.ma09name, cityText));
                if (query.Any())
                {
                    return query.ToList();
                }
            }
            else
            {
                var query = _DbContext.ma09city.Include(e => e.ma08uf)
                .Where(e => TextManipulation.RemoveAccents(e.ma09name, cityText) && e.ma05trip.Any());
                if (query.Any())
                {
                    return query.ToList();
                }
            }
            return null;
        }
    }
}
