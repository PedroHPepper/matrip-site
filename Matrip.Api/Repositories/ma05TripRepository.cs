using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace Matrip.Web.Repositories
{
    public class ma05TripRepository : BaseRepository<ma05trip>, Ima05TripRepository
    {
        private int _registersPerPage = 9;
        public ma05TripRepository(ApplicationDbContext DbContext) : base(DbContext)
        {
        }
        public List<ma05trip> GetFeaturedTrips()
        {
            var query = _DbContext.ma05trip.AsNoTracking().Where(e => e.ma05featuredtour == "1")
                .Include(e => e.ma13tripphoto);
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }
        public IPagedList<ma05trip> GetList(int CategoryId, int CityId, int? page)
        {
            int pageNumber = page ?? 1;
            if (CategoryId == 0)
            {
                return _DbContext.ma05trip.AsNoTracking().Where<ma05trip>(e =>
                    e.FK0509idcity == CityId && e.ma05status == "1")
                    .Include(e => e.ma13tripphoto)
                    .Include(e => e.ma14subtrip).ThenInclude(e => e.ma16subtripschedule)
                    .Include(e => e.ma09city).ThenInclude(e => e.ma08uf)
                    .Include(e => e.ma09city).ThenInclude(e => e.ma35cityphoto)
                .ToPagedList<ma05trip>(pageNumber, _registersPerPage);
            }
            else
            {
                return _DbContext.ma05trip.AsNoTracking().Where<ma05trip>(e =>
                    e.FK0506idtripcategory == CategoryId &&
                    e.FK0509idcity == CityId && e.ma05status == "1")
                    .Include(e => e.ma13tripphoto)
                    .Include(e => e.ma14subtrip).ThenInclude(e => e.ma16subtripschedule)
                    .Include(e => e.ma09city).ThenInclude(e => e.ma08uf)
                    .Include(e => e.ma09city).ThenInclude(e => e.ma35cityphoto)
                    .Include(e => e.ma06tripcategory)
                .ToPagedList<ma05trip>(pageNumber, _registersPerPage);
            }
        }
        public ma05trip GetTrip(int TripID)
        {
            var query = _DbContext.ma05trip.AsNoTracking()
                .Include(e => e.ma13tripphoto)
                .Include(e => e.ma27AgeDiscount)
                .Include(e => e.ma09city).ThenInclude(e => e.ma35cityphoto)
                .Include(e => e.ma09city).ThenInclude(e => e.ma08uf)
                .Include(e => e.ma06tripcategory)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma15subtripphoto)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma16subtripschedule)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma17SubtripValue)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma11service)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma25partner)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma12SubtripGuide).ThenInclude(e => e.ma04guide)
                .Include(e => e.ma39tripEvaluation)
                .Where(e => e.ma05idtrip == TripID);
            if (query.Any())
            {
                return query.First();
            }
            return null;
        }
        public ma05trip GetTrip(string TripName)
        {
            var query = _DbContext.ma05trip.AsNoTracking()
                .Include(e => e.ma13tripphoto)
                .Include(e => e.ma27AgeDiscount)
                .Include(e => e.ma09city).ThenInclude(e => e.ma35cityphoto)
                .Include(e => e.ma09city).ThenInclude(e => e.ma08uf)
                .Include(e => e.ma06tripcategory)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma15subtripphoto)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma16subtripschedule)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma17SubtripValue)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma11service)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma25partner)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma12SubtripGuide).ThenInclude(e => e.ma04guide)
                .Where(e => e.ma05name == TripName);
            if (query.Any())
            {
                return query.First();
            }
            return null;
        }

        public List<ma05trip> GetSearchTrip(string TripNameText)
        {
            var query = _DbContext.ma05trip.AsNoTracking().Include(e => e.ma09city)
                            .Where(e => TextManipulation.RemoveAccents(e.ma05name, TripNameText)
                            || TextManipulation.RemoveAccents(e.ma09city.ma09name, TripNameText));
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }

        public List<ma05trip> GetGuideTripList(int GuideID)
        {
            var query = _DbContext.ma05trip.Include(e => e.ma14subtrip).ThenInclude(e => e.ma12SubtripGuide).
                Where(e => e.ma14subtrip.Where(w => w.ma12SubtripGuide.Where(p => p.ma12idSubtripGuide == GuideID).ToList() != null).ToList() != null);
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }
        public ma05trip GetGuideTrip(int TripID)
        {
            var query = _DbContext.ma05trip.AsNoTracking().
                Include(e => e.ma27AgeDiscount).
                Include(e => e.ma09city).
                Include(e => e.ma06tripcategory).
                Include(e => e.ma14subtrip).ThenInclude(e => e.ma16subtripschedule).
                Include(e => e.ma14subtrip).ThenInclude(e => e.ma17SubtripValue).
                Include(e => e.ma14subtrip).ThenInclude(e => e.ma11service).
                Include(e => e.ma14subtrip).ThenInclude(e => e.ma12SubtripGuide).ThenInclude(e => e.ma04guide).
                Where(e => e.ma05idtrip == TripID);
            if (query.Any())
            {
                return query.First();
            }
            return null;
        }


        public ma05trip GetEvaluatedTrip(string TripName)
        {
            var query = _DbContext.ma05trip.AsNoTracking()
                .Include(e => e.ma39tripEvaluation)
                .Where(e => e.ma05name == TripName);
            if (query.Any())
            {
                return query.First();
            }
            return null;
        }
    }
}
