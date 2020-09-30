using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma06TripCategoryRepository : BaseRepository<ma06tripcategory>, Ima06TripCategoryRepository
    {
        public ma06TripCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public ma06tripcategory GetByName(string CategoryName)
        {
            return _DbContext.ma06tripcategory.Where(e => e.ma06name == CategoryName).FirstOrDefault();
        }
    }
}
