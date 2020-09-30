using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;

namespace Matrip.Web.Repositories
{
    public class ma34TransferencePendenciesRepository : BaseRepository<ma34TransferencePendencies>, Ima34TransferencePendenciesRepository
    {
        public ma34TransferencePendenciesRepository(ApplicationDbContext ApplicationDbContext) : base(ApplicationDbContext)
        {
        }
    }
}
