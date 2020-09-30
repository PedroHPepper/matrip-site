using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima30GuideSubtripShoppingCartRepository : IBaseRepository<ma30GuideSubtripShoppingCart>
    {
        List<ma30GuideSubtripShoppingCart> GetSubTripShoppingCartGuideList(int SubTripShoppingCartID);
    }
}
