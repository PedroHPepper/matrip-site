using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima12SubtripGuideRepository : IBaseRepository<ma12SubtripGuide>
    {
        List<ma12SubtripGuide> GetBySubTripID(int SubtripID);
    }
}
