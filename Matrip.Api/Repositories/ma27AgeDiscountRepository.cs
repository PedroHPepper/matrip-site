using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma27AgeDiscountRepository : BaseRepository<ma27AgeDiscount>, Ima27AgeDiscountRepository
    {
        public ma27AgeDiscountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<ma27AgeDiscount> GetListByTripID(int TripID)
        {
            var query = _DbContext.ma27AgeDiscount.Where(e => e.FK2705idTrip == TripID);
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }
    }
}
