using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;

namespace Matrip.Web.Repositories
{
    public class ma28SaleTouristRepository : BaseRepository<ma28SaleTourist>, Ima28SaleTouristRepository
    {
        public ma28SaleTouristRepository(ApplicationDbContext ApplicationDbContext) : base(ApplicationDbContext)
        {
        }
    }
}
