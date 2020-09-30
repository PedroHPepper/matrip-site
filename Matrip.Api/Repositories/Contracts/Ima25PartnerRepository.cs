using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima25PartnerRepository : IBaseRepository<ma25partner>
    {
        ma25partner GetPartnerWithSubtrips(int partnerID, int? guideID);
        ma25partner GetPartnerWithSubtrips(string PartnerName, int? guideID);
        ma25partner GetPartnerWithGuideList(int PartnerID);
        List<ma25partner> GetSearchPartner(string PartnerText);
        ma25partner GetByName(string PartnerName);
    }
}
