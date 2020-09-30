using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma29TouristShoppingCartRepository : BaseRepository<ma29TouristShoppingCart>, Ima29TouristShoppingCartRepository
    {
        public ma29TouristShoppingCartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<ma29TouristShoppingCart> GetTouristListByTripItemShoppingCartID(int TripItemShoppingCartID)
        {
            var query = _DbContext.ma29TouristShoppingCart.Where(e => e.FK2918idTripItemShoppingCart == TripItemShoppingCartID);
            if (query.Any())
            {
                query.ToList();
            }
            return null;
        }
    }
}
