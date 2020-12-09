using Matrip.Domain.Models.Entities;
using System.Collections.Generic;
using X.PagedList;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima05TripRepository : IBaseRepository<ma05trip>
    {
        List<ma05trip> GetFeaturedTrips();
        IPagedList<ma05trip> GetList(int CategoryId, int CityId, int? page);
        ma05trip GetTrip(int TripID);
        ma05trip GetTrip(string TripName);
        List<ma05trip> GetSearchTrip(string TripNameText);
        List<ma05trip> GetGuideTripList(int GuideID);

        ma05trip GetEvaluatedTrip(string TripName);
    }
}
