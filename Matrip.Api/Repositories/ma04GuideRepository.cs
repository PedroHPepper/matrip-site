using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Matrip.Web.Repositories
{
    public class ma04GuideRepository : BaseRepository<ma04guide>, Ima04GuideRepository
    {
        public ma04GuideRepository(ApplicationDbContext DbContext) : base(DbContext)
        {

        }

        public ma04guide GetGuideByUserId(int userID)
        {
            var query = _DbContext.Set<ma04guide>().Where(e => e.FK0401iduser == userID);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
        public ma04guide GetGuidePartnerList(int userID)
        {
            var query = _DbContext.ma04guide.Where(e => e.FK0401iduser == userID)
                .Include(e => e.ma26PartnerGuide).ThenInclude(e => e.ma25partner);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
        /*
        public ma04guide GetGuideReport(int userID, DateTime initialDate, DateTime finalDate)
        {
            var query = _DbContext.ma04guide.AsNoTracking().Where(e => e.FK0401iduser == userID)
                .Include(e => e.ma31SubtripSaleGuide).ThenInclude(e => e.ma22subtripsale)
                    .ThenInclude(e => e.ma23servicesale).ThenInclude(e => e.ma11service)
                .Include(e => e.ma31SubtripSaleGuide).ThenInclude(e => e.ma22subtripsale).ThenInclude(e => e.ma14subtrip).ThenInclude(e => e.ma25partner)
                .Include(e => e.ma31SubtripSaleGuide).ThenInclude(e => e.ma22subtripsale)
                    .ThenInclude(e => e.ma21saleTrip).ThenInclude(e => e.ma32sale).ThenInclude(e=>e.ma01user)
                .Include(e => e.ma26PartnerGuide).ThenInclude(e => e.ma25partner)
                .IncludeFilter(e => e.ma31SubtripSaleGuide.Where(w => w.ma22subtripsale.ma22Date >= initialDate && w.ma22subtripsale.ma22Date <= finalDate));
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }*/
        public ma04guide GetGuideReport(int userID, int partnerID, DateTime initialDate, DateTime finalDate)
        {
            var query = _DbContext.ma04guide.Where(e => e.FK0401iduser == userID)
                .Include(e => e.ma26PartnerGuide).ThenInclude(e => e.ma25partner).ThenInclude(e => e.ma14subtrip).
                    ThenInclude(e => e.ma22subtripsale).ThenInclude(e => e.ma23servicesale).ThenInclude(e => e.ma11service)
                .Include(e => e.ma26PartnerGuide).ThenInclude(e => e.ma25partner).ThenInclude(e => e.ma14subtrip)
                    .ThenInclude(e => e.ma22subtripsale).ThenInclude(e => e.ma21saleTrip)
                    .ThenInclude(e => e.ma32sale).ThenInclude(e => e.ma01user)
                .IncludeFilter(e => e.ma26PartnerGuide.
                        Where(w => w.FK2625idPartner == partnerID && w.ma25partner.ma14subtrip.Where(s => s.ma22subtripsale.Where(d => d.ma22Date >= initialDate && d.ma22Date <= finalDate).Any()).Any()).FirstOrDefault());//(w => w.ma22subtripsale.ma22Date >= initialDate && w.ma22subtripsale.ma22Date <= finalDate));
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
        public ma25partner GetPartnerReport(int userID, int partnerID, DateTime initialDate, DateTime finalDate)
        {
            var query = _DbContext.ma25partner.Where(e => e.ma25idpartner == partnerID)
                .Include(e => e.ma26PartnerGuide)
                .Include(e => e.ma14subtrip).
                    ThenInclude(e => e.ma22subtripsale).ThenInclude(e => e.ma23servicesale).ThenInclude(e => e.ma11service)
                .Include(e => e.ma14subtrip)
                    .ThenInclude(e => e.ma22subtripsale).ThenInclude(e => e.ma21saleTrip)
                    .ThenInclude(e => e.ma32sale).ThenInclude(e => e.ma01user)
                    .IncludeFilter(e => e.ma14subtrip.Where(s => s.ma22subtripsale.Where(d => d.ma22Date >= initialDate && d.ma22Date <= finalDate).Any()));

            return null;
        }
    }
}
