using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma18TripItemShoppingCartRepository : BaseRepository<ma18tripitemshoppingcart>, Ima18TripItemShoppingCartRepository
    {
        public ma18TripItemShoppingCartRepository(ApplicationDbContext _DbContext) : base(_DbContext)
        {
        }


        public List<ma18tripitemshoppingcart> GetTripItemShoppingCartList(int UserID)
        {
            var query = _DbContext.ma18tripitemshoppingcart.AsNoTracking().Where(e => e.FK1801iduser == UserID).
                Include(e => e.ma29TouristShoppingCart)
                .ThenInclude(e => e.ma27AgeDiscount)
                .Include(e => e.ma05trip)
                    .ThenInclude(e => e.ma27AgeDiscount)
                .Include(e => e.ma05trip)
                    .ThenInclude(e => e.ma06tripcategory)
                .Include(e => e.ma05trip)
                    .ThenInclude(e => e.ma09city)
                .Include(e => e.ma29TouristShoppingCart)
                .Include(e => e.ma19SubTripItemShoppingCart)
                    .ThenInclude(e => e.ma14subtrip).ThenInclude(e => e.ma16subtripschedule)
                .Include(e => e.ma19SubTripItemShoppingCart)
                    .ThenInclude(e => e.ma17SubtripValue)
                .Include(e => e.ma19SubTripItemShoppingCart)
                    .ThenInclude(e => e.ma20ServiceItemShoppingCart).ThenInclude(e => e.ma11service);
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }
        public ma18tripitemshoppingcart GetTripItemShoppingCart(int TripItemShoppingCartID)
        {
            var query = _DbContext.ma18tripitemshoppingcart.AsNoTracking().Where(e => e.ma18idtripitemshoppingcart == TripItemShoppingCartID).
                Include(e => e.ma05trip).
                    ThenInclude(e => e.ma27AgeDiscount).
                Include(e => e.ma05trip).
                    ThenInclude(e => e.ma09city).
                Include(e => e.ma29TouristShoppingCart).
                Include(e => e.ma19SubTripItemShoppingCart).
                    ThenInclude(e => e.ma14subtrip).
                Include(e => e.ma19SubTripItemShoppingCart).
                    ThenInclude(e => e.ma20ServiceItemShoppingCart).ThenInclude(e => e.ma11service);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }


    }
}
