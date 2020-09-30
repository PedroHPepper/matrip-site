using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima29TouristShoppingCartRepository : IBaseRepository<ma29TouristShoppingCart>
    {
        List<ma29TouristShoppingCart> GetTouristListByTripItemShoppingCartID(int TripItemShoppingCartID);
    }
}
