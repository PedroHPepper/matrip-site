using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima20ServiceItemShoppingCartRepository : IBaseRepository<ma20ServiceItemShoppingCart>
    {
        List<ma20ServiceItemShoppingCart> GetServiceItemShoppingCartList(int SubTripItemShoppingCartID);
    }
}
