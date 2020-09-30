using Matrip.Domain.Models.Entities;
using System;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima04GuideRepository : IBaseRepository<ma04guide>
    {
        ma04guide GetGuideByUserId(int userID);
        ma04guide GetGuidePartnerList(int userID);
        ma04guide GetGuideReport(int userID, int partnerID, DateTime initialDate, DateTime finalDate);
    }
}
