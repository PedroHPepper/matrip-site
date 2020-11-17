using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using X.PagedList;

namespace Matrip.Web.Repositories
{
    public class ma32saleRepository : BaseRepository<ma32sale>, Ima32saleRepository
    {
        private int _registroPaginas = 15;
        public ma32saleRepository(ApplicationDbContext ApplicationDbContext) : base(ApplicationDbContext)
        {
        }
        public IPagedList<ma32sale> GetSaleList(int userID, int? page)
        {
            int pageNumber = page ?? 1;
            var query = _DbContext.ma32sale.Where(e => e.FK3201iduser == userID).OrderByDescending(e => e.ma32SaleDate).
                Include(e => e.ma24payment);
            
            return query.ToPagedList<ma32sale>(pageNumber, _registroPaginas);
        }
        public ma32sale GetSale(int SaleID)
        {
            var query = _DbContext.ma32sale.Where(e => e.ma32idSale == SaleID)
                .Include(e => e.ma24payment)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma05trip).ThenInclude(e => e.ma27AgeDiscount)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma05trip).ThenInclude(e => e.ma09city).ThenInclude(e => e.ma08uf)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma22subtripsale)
                    .ThenInclude(e => e.ma14subtrip).ThenInclude(e => e.ma17SubtripValue)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma22subtripsale)
                    .ThenInclude(e => e.ma14subtrip).ThenInclude(e => e.ma25partner).ThenInclude(e => e.ma09city).ThenInclude(e => e.ma08uf)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma22subtripsale).ThenInclude(e => e.ma23servicesale).ThenInclude(e => e.ma11service)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma22subtripsale).ThenInclude(e => e.ma31SubtripSaleGuide)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma28SaleTourist);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }

        public ma32sale GetLastSale()
        {
            var query = _DbContext.ma32sale.OrderByDescending(e => e.ma32SaleDate)
                .Include(e => e.ma24payment)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma05trip).ThenInclude(e => e.ma27AgeDiscount)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma05trip).ThenInclude(e => e.ma09city).ThenInclude(e => e.ma08uf)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma22subtripsale)
                    .ThenInclude(e => e.ma14subtrip).ThenInclude(e => e.ma17SubtripValue)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma22subtripsale)
                    .ThenInclude(e => e.ma14subtrip).ThenInclude(e => e.ma25partner).ThenInclude(e => e.ma09city).ThenInclude(e => e.ma08uf)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma22subtripsale).ThenInclude(e => e.ma23servicesale).ThenInclude(e => e.ma11service)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma22subtripsale).ThenInclude(e => e.ma31SubtripSaleGuide)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma28SaleTourist);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }

        public ma32sale GetSaleToFeedback(int SaleID)
        {
            var query = _DbContext.ma32sale.Where(e => e.ma32idSale == SaleID)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma05trip).ThenInclude(e => e.ma09city).ThenInclude(e => e.ma08uf)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma22subtripsale)
                    .ThenInclude(e => e.ma14subtrip).ThenInclude(e => e.ma17SubtripValue)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma39tripEvaluation)
                .Include(e => e.ma21saleTrip).ThenInclude(e => e.ma22subtripsale).ThenInclude(e => e.ma14subtrip);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
    }
}
