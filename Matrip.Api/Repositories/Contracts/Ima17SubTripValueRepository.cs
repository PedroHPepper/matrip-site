
using Matrip.Domain.Models.Entities;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima17SubTripValueRepository : IBaseRepository<ma17SubtripValue>
    {
        ma17SubtripValue GetSubTripValue(int SubTripID);
    }
}
