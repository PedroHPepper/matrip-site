using Matrip.Domain.Models.Entities;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima33UserAddressRepository : IBaseRepository<ma33UserAddress>
    {
        ma33UserAddress GetByUserID(int UserID);
        void AddOrUpdate(int UserID, ma33UserAddress userAddress);
    }
}
