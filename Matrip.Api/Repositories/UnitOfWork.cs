using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Threading.Tasks;

namespace Matrip.Web.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
