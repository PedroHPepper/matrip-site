using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma15SubTripPhotoRepository : BaseRepository<ma15subtripphoto>, Ima15SubTripPhotoRepository
    {
        public ma15SubTripPhotoRepository(ApplicationDbContext _DbContext) : base(_DbContext)
        {
        }

        public List<ma15subtripphoto> GetSubTripPhotoListByTrip(int SubTripID)
        {
            return _DbContext.ma15subtripphoto.Where(e => e.FK1514idsubtrip == SubTripID).ToList();
        }

    }
}
