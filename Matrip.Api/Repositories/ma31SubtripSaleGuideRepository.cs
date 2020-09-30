using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;

namespace Matrip.Web.Repositories
{
    public class ma31SubtripSaleGuideRepository : BaseRepository<ma31SubtripSaleGuide>, Ima31SubtripSaleGuideRepository
    {
        public ma31SubtripSaleGuideRepository(ApplicationDbContext ApplicationDbContext) : base(ApplicationDbContext)
        {
        }
    }
}
