using Matrip.Domain.Models.Entities;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima26PartnerGuideRepository : IBaseRepository<ma26PartnerGuide>
    {
        bool AddPartnerGuide(ma26PartnerGuide partnerGuide);
    }
}
