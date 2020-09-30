using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;

namespace Matrip.Web.Repositories
{
    public class ma23ServiceSaleRepository : BaseRepository<ma23servicesale>, Ima23ServiceSaleRepository
    {
        public ma23ServiceSaleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
