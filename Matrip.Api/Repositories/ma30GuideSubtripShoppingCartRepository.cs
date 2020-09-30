using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma30GuideSubtripShoppingCartRepository : BaseRepository<ma30GuideSubtripShoppingCart>, Ima30GuideSubtripShoppingCartRepository
    {
        public ma30GuideSubtripShoppingCartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<ma30GuideSubtripShoppingCart> GetSubTripShoppingCartGuideList(int SubTripShoppingCartID)
        {
            var query = _DbContext.ma30GuideSubtripShoppingCart.Where(e => e.FK3019idsubtripitemshoppingcart == SubTripShoppingCartID);
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }
    }
}
