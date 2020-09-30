using Matrip.Domain.Models.Entities;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima09CityRepository : IBaseRepository<ma09city>
    {
        ma09city GetByName(int UFid, string City);
        List<ma09city> GetCities();
        ma09city GetCityByID(int CityID);
        List<ma09city> GetSearch(string cityText, string UF);
    }
}
