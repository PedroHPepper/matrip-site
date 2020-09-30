using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma13TripPhotoRepository : BaseRepository<ma13tripphoto>, Ima13TripPhotoRepository
    {
        public ma13TripPhotoRepository(ApplicationDbContext _DbContext) : base(_DbContext)
        {
        }

        public List<ma13tripphoto> GetTripPhotoListByTrip(int TripID)
        {
            return _DbContext.ma13tripphoto.Where(e => e.FK1305idtrip == TripID).ToList();
        }

        public ma13tripphoto GetByTripID(int TripID)
        {
            var query = _DbContext.ma13tripphoto.Where(e => e.FK1305idtrip == TripID);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
    }
}
