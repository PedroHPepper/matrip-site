using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima14SubTripRepository : IBaseRepository<ma14subtrip>
    {
        List<ma14subtrip> GetSubTripList(int TripID);
    }
}
