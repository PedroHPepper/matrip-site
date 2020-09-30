using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima19SubTripItemShoppingCartRepository : IBaseRepository<ma19SubTripItemShoppingCart>
    {
        List<ma19SubTripItemShoppingCart> GetSubTripItemShoppingCartList(int TripItemShoppingCartID);
    }
}
