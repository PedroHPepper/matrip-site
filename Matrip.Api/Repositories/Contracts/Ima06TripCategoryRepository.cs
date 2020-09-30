
using Matrip.Domain.Models.Entities;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima06TripCategoryRepository : IBaseRepository<ma06tripcategory>
    {
        ma06tripcategory GetByName(string CategoryName);
    }
}
