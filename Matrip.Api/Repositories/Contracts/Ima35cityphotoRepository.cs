using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima35cityphotoRepository : IBaseRepository<ma35cityphoto>
    {
        ma35cityphoto GetCityPhoto(int CityID);
    }
}
