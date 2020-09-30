using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima13TripPhotoRepository : IBaseRepository<ma13tripphoto>
    {
        List<ma13tripphoto> GetTripPhotoListByTrip(int TripID);
        ma13tripphoto GetByTripID(int TripID);
    }
}
