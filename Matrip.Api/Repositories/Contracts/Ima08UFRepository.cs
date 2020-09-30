
using Matrip.Domain.Models.Entities;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima08UFRepository : IBaseRepository<ma08uf>
    {
        ma08uf GetByInitials(string UF);
    }
}
