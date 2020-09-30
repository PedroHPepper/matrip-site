using System.Threading.Tasks;

namespace Matrip.Web.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        Task Commit();
        void Dispose();
    }
}
