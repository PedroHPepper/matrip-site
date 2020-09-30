using Matrip.Domain.Models.Entities;
using System.Threading.Tasks;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima01UserRepository : IBaseRepository<ma01user>
    {

        Task<string> Register(ma01user ma01user, string password);
        Task ConfirmEmail(ma01user ma01user, string code);
        Task ResetPassword(ma01user ma01user, string code, string Password);
        Task<ma01user> Login(string Email, string password);
        Task<string> GetPasswordResetToken(ma01user ma01user);

        Task<ma01user> GetByIdAsync(int id);
        Task<ma01user> GetByEmailAsync(string email);
        Task UpdateUser(ma01user ma01user);
        Task ChangePassword(ma01user ma01user, string password, string newPassword);

    }
}
