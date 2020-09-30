using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;

namespace Matrip.Web.Repositories
{
    public class ma24PaymentRepository : BaseRepository<ma24payment>, Ima24PaymentRepository
    {
        public ma24PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
