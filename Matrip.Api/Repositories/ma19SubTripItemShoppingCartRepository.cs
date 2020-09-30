using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma19SubTripItemShoppingCartRepository : BaseRepository<ma19SubTripItemShoppingCart>, Ima19SubTripItemShoppingCartRepository
    {
        public ma19SubTripItemShoppingCartRepository(ApplicationDbContext _DbContext) : base(_DbContext)
        {
        }


        public List<ma19SubTripItemShoppingCart> GetSubTripItemShoppingCartList(int TripItemShoppingCartID)
        {
            return _DbContext.ma19SubTripItemShoppingCart.Where(e => e.FK1918idTripItemShoppingCart == TripItemShoppingCartID).ToList();
        }


        public void RemoveSubTripItemShoppingCartList(List<ma19SubTripItemShoppingCart> subTripItemShoppingCart)
        {
            _DbContext.RemoveRange(subTripItemShoppingCart);

            _DbContext.SaveChanges();
        }
    }
}
