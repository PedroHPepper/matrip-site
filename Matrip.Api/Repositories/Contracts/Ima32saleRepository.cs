using Matrip.Domain.Models.Entities;
using X.PagedList;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima32saleRepository : IBaseRepository<ma32sale>
    {
        IPagedList<ma32sale> GetSaleList(int userID, int? page);
        ma32sale GetSale(int SaleID);
        ma32sale GetLastSale();
    }
}
