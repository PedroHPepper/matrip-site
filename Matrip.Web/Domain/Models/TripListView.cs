
using Matrip.Domain.Models.Entities;
using Matrip.Web.Libraries.Pagination;

namespace Matrip.Web.Domain.Models
{
    public class TripListView
    {
        public PaginationEntity<ma05trip> lst { get; set; }
        public ma09city ma09city { get; set; }
        public ma06tripcategory ma06tripcategory { get; set; }
        public ma08uf ma08uf { get; set; }
    }
}
