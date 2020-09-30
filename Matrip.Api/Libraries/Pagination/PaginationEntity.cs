using System.Collections.Generic;

namespace Matrip.Web.Libraries.Pagination
{
    public class PaginationEntity<T> where T : class
    {
        public PaginationEntity()
        {
            Items = new List<T>();
        }
        public IEnumerable<T> Items { get; set; }
        public PaginationMetaData MetaData { get; set; }
    }
}
