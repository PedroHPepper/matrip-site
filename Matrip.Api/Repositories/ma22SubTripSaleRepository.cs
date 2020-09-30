using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma22SubTripSaleRepository : BaseRepository<ma22subtripsale>, Ima22SubTripSaleRepository
    {
        public ma22SubTripSaleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public List<ma22subtripsale> GetSubtripSaleList(ma16subtripschedule ma16Subtripschedule, DateTime Date, int ValueID)
        {
            var query = _DbContext.ma22subtripsale.Where(e => e.ma22Entry == ma16Subtripschedule.ma16entry
                && e.FK2214idSubTrip == ma16Subtripschedule.FK1614idsubtrip && e.ma22Date == Date)
                .Include(e => e.ma17SubtripValue)
                .Where(e => e.ma21saleTrip.ma32sale.ma32situation != 2)
                .Include(e => e.ma14subtrip);

            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }

        public List<ma22subtripsale> GetSubtripReport(ma14subtrip subtrip, DateTime initialDate, DateTime finalDate, int DateType)
        {
            if(DateType == 1)
            {
                var query = _DbContext.ma22subtripsale.AsNoTracking()
                .Where(e => e.ma22SaleDate >= initialDate && e.ma22SaleDate <= finalDate && e.FK2214idSubTrip == subtrip.ma14idsubtrip)
                .Include(e => e.ma21saleTrip)
                    .ThenInclude(e => e.ma32sale).ThenInclude(e => e.ma01user)
                .Include(e => e.ma21saleTrip)
                    .ThenInclude(e => e.ma32sale).ThenInclude(e => e.ma24payment)
                .Include(e => e.ma14subtrip)
                .Include(e => e.ma23servicesale).ThenInclude(e => e.ma11service);
                if (query.Any())
                {
                    return query.ToList();
                }
            }
            else
            {
                var query = _DbContext.ma22subtripsale.AsNoTracking()
                .Where(e => e.ma22Date >= initialDate && e.ma22Date <= finalDate && e.FK2214idSubTrip == subtrip.ma14idsubtrip)
                .Include(e => e.ma21saleTrip)
                    .ThenInclude(e => e.ma32sale).ThenInclude(e => e.ma01user)
                .Include(e => e.ma21saleTrip)
                    .ThenInclude(e => e.ma32sale).ThenInclude(e => e.ma24payment)
                .Include(e => e.ma14subtrip)
                .Include(e => e.ma23servicesale).ThenInclude(e => e.ma11service);
                if (query.Any())
                {
                    return query.ToList();
                }
            }
            return null;
        }

        public List<ma22subtripsale> GetSubtripReport(DateTime initialDate, DateTime finalDate, int DateType)
        {
            if(DateType == 1)
            {
                var query = _DbContext.ma22subtripsale.AsNoTracking()
                .Where(e => e.ma22SaleDate >= initialDate && e.ma22SaleDate <= finalDate)
                .Include(e => e.ma21saleTrip)
                    .ThenInclude(e => e.ma32sale).ThenInclude(e => e.ma01user)
                .Include(e => e.ma21saleTrip)
                    .ThenInclude(e => e.ma32sale).ThenInclude(e => e.ma24payment)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma25partner)
                .Include(e => e.ma23servicesale).ThenInclude(e => e.ma11service);
                if (query.Any())
                {
                    return query.ToList();
                }
            }
            else
            {
                var query = _DbContext.ma22subtripsale.AsNoTracking()
                .Where(e => e.ma22Date >= initialDate && e.ma22Date <= finalDate)
                .Include(e => e.ma21saleTrip)
                    .ThenInclude(e => e.ma32sale).ThenInclude(e => e.ma01user)
                .Include(e => e.ma21saleTrip)
                    .ThenInclude(e => e.ma32sale).ThenInclude(e => e.ma24payment)
                .Include(e => e.ma14subtrip).ThenInclude(e => e.ma25partner)
                .Include(e => e.ma23servicesale).ThenInclude(e => e.ma11service);
                if (query.Any())
                {
                    return query.ToList();
                }
            }
            return null;
        }
    }
}
