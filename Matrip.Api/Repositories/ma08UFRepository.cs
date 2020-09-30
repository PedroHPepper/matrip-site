using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma08UFRepository : BaseRepository<ma08uf>, Ima08UFRepository
    {
        public ma08UFRepository(ApplicationDbContext DbContext) : base(DbContext)
        {

        }

        public ma08uf GetByInitials(string UF)
        {
            var query = _DbContext.Set<ma08uf>().Where(e => e.ma08UFInitials == UF);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
    }
}
