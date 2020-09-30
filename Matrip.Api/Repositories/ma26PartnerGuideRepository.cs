using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma26PartnerGuideRepository : BaseRepository<ma26PartnerGuide>, Ima26PartnerGuideRepository
    {
        public ma26PartnerGuideRepository(ApplicationDbContext ApplicationDbContext) : base(ApplicationDbContext)
        {
        }

        public bool AddPartnerGuide(ma26PartnerGuide partnerGuide)
        {
            var query = _DbContext.ma26PartnerGuide
                .Where(e => e.FK2604idGuide == partnerGuide.FK2604idGuide && e.FK2625idPartner == partnerGuide.FK2625idPartner);
            if (query.Any())
            {
                return false;
            }
            else
            {
                _DbContext.ma26PartnerGuide.Add(partnerGuide);
                return true;
            }
        }
    }
}
