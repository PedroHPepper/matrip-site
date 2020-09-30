using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;

namespace Matrip.Web.Repositories
{
    public class ma21SaleTripRepository : BaseRepository<ma21saleTrip>, Ima21SaleTripRepository
    {
        public ma21SaleTripRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
