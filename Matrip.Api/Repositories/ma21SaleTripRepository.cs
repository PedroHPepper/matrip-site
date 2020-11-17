using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma21SaleTripRepository : BaseRepository<ma21saleTrip>, Ima21SaleTripRepository
    {
        public ma21SaleTripRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ma21saleTrip GetSaleTripToFeedback(int SaleTripID)
        {
            var query = _DbContext.ma21saleTrip.Where(e => e.ma21idSaleTrip == SaleTripID)
                .Include(e => e.ma05trip).ThenInclude(e => e.ma09city).ThenInclude(e => e.ma08uf)
                .Include(e => e.ma39tripEvaluation)
                .Include(e => e.ma22subtripsale)
                    .ThenInclude(e => e.ma14subtrip).ThenInclude(e => e.ma17SubtripValue)
                .Include(e => e.ma22subtripsale).ThenInclude(e => e.ma14subtrip);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
    }
}
