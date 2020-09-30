using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma20ServiceItemShoppingCartRepository : BaseRepository<ma20ServiceItemShoppingCart>, Ima20ServiceItemShoppingCartRepository
    {
        public ma20ServiceItemShoppingCartRepository(ApplicationDbContext _DbContext) : base(_DbContext)
        {
        }

        public List<ma20ServiceItemShoppingCart> GetServiceItemShoppingCartList(int SubTripItemShoppingCartID)
        {
            return _DbContext.ma20ServiceItemShoppingCart.Where(e => e.FK2019idSubTripItemShoppingCart == SubTripItemShoppingCartID).ToList();
        }

        public void RemoveSubTripItemShoppingCartList(List<ma20ServiceItemShoppingCart> serviceItemShoppingCart)
        {
            _DbContext.RemoveRange(serviceItemShoppingCart);

            _DbContext.SaveChanges();
        }
    }
}
