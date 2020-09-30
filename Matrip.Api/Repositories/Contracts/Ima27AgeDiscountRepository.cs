using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima27AgeDiscountRepository : IBaseRepository<ma27AgeDiscount>
    {
        List<ma27AgeDiscount> GetListByTripID(int TripID);
    }
}
