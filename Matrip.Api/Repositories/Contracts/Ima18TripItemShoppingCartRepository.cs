using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima18TripItemShoppingCartRepository : IBaseRepository<ma18tripitemshoppingcart>
    {
        List<ma18tripitemshoppingcart> GetTripItemShoppingCartList(int UserID);
        ma18tripitemshoppingcart GetTripItemShoppingCart(int TripItemShoppingCartID);
    }
}
