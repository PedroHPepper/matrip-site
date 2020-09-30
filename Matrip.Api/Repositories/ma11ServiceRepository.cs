using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace Matrip.Web.Repositories
{
    public class ma11ServiceRepository : BaseRepository<ma11service>, Ima11ServiceRepository
    {
        private int _registroPaginas = 25;
        public ma11ServiceRepository(ApplicationDbContext DbContext) : base(DbContext)
        {
        }

        public IPagedList<ma11service> GetList(int? page)
        {
            int pageNumber = page ?? 1;
            return _DbContext.ma11service.ToPagedList<ma11service>(pageNumber, _registroPaginas);
        }

        public void DeleteAllServicesInSubTrip(int SubTripID)
        {
            var query = _DbContext.Set<ma11service>().Where(e => e.FK1114idsubtrip == SubTripID);
            if (query.Any())
            {
                _DbContext.ma11service.RemoveRange(query);

                _DbContext.SaveChanges();
            }
        }
        public List<ma11service> GetSubTripServiceList(int SubTripID)
        {
            return _DbContext.Set<ma11service>().Where(e => e.FK1114idsubtrip == SubTripID).ToList();
        }
    }
}
