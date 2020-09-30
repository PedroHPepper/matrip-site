using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma25PartnerRepository : BaseRepository<ma25partner>, Ima25PartnerRepository
    {
        public ma25PartnerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ma25partner GetByName(string PartnerName)
        {
            var query = _DbContext.ma25partner.Where(e => e.ma25name == PartnerName);

            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }

        public ma25partner GetPartnerWithSubtrips(int partnerID, int? guideID)
        {
            var query = _DbContext.ma25partner
                .Include(e => e.ma14subtrip)
                .Include(e => e.ma26PartnerGuide)
                .Where(e => e.ma25idpartner == partnerID);
            if (query.Any())
            {
                ma25partner partner = query.FirstOrDefault();
                if(guideID == null)
                {
                    return partner;
                }
                else
                {
                    if (partner.ma26PartnerGuide.Where(e => e.FK2604idGuide == guideID.Value).Any())
                    {
                        return partner;
                    }
                }
            }
            return null;
        }

        public ma25partner GetPartnerWithSubtrips(string PartnerName, int? guideID)
        {
            var query = _DbContext.ma25partner
                .Include(e => e.ma14subtrip)
                .Include(e => e.ma26PartnerGuide)
                .Where(e => e.ma25name == PartnerName);
            if (query.Any())
            {
                ma25partner partner = query.FirstOrDefault();
                if (guideID == null)
                {
                    return partner;
                }
                else
                {
                    if (partner.ma26PartnerGuide.Where(e => e.FK2604idGuide == guideID.Value).Any())
                    {
                        return partner;
                    }
                }
            }
            return null;
        }

        public List<ma25partner> GetSearchPartner(string PartnerText)
        {
            var query = _DbContext.ma25partner.Where(e => TextManipulation.RemoveAccents(e.ma25name, PartnerText));

            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }

        public ma25partner GetPartnerWithGuideList(int PartnerID)
        {
            var query = _DbContext.ma25partner.Where(e => e.ma25idpartner == PartnerID)
                .Include(e => e.ma26PartnerGuide).ThenInclude(e => e.ma04guide).ThenInclude(e => e.ma01user)
                .Include(e => e.ma09city).ThenInclude(e => e.ma08uf);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }

    }
}
