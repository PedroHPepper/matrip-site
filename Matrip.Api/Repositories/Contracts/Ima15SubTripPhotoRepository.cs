using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima15SubTripPhotoRepository : IBaseRepository<ma15subtripphoto>
    {
        List<ma15subtripphoto> GetSubTripPhotoListByTrip(int SubTripID);
    }
}
