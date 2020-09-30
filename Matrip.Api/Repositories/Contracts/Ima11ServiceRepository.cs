using Matrip.Domain.Models.Entities;
using System.Collections.Generic;
using X.PagedList;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima11ServiceRepository : IBaseRepository<ma11service>
    {
        IPagedList<ma11service> GetList(int? page);
        void DeleteAllServicesInSubTrip(int SubTripID);
        List<ma11service> GetSubTripServiceList(int SubTripID);
    }
}
