using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Matrip.Web.Repositories
{
    public class ma01UserRepository : BaseRepository<ma01user>, Ima01UserRepository
    {
        private readonly UserManager<ma01user> _userManager;

        public ma01UserRepository(ApplicationDbContext DbContext, UserManager<ma01user> userManager) : base(DbContext)
        {
            _userManager = userManager;
        }
        /*
        public async Task<string> Register(ma01user ma01user, string password)
        {
            var result = await _userManager.CreateAsync(ma01user, password);
            if (result.Succeeded)
            {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(ma01user);
                return code;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    sb.Append(error.Description);
                }
                throw new Exception($"Erro ao cadastrar!! {sb.ToString()}");
            }
        }
        */
        public async Task<string> Register(ma01user ma01user, string password)
        {
            var result = await _userManager.CreateAsync(ma01user, password);
            if (result.Succeeded)
            {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(ma01user);
                return code;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    sb.Append(error.Description);
                }
                throw new Exception($"Erro ao cadastrar!! {sb.ToString()}");
            }
        }
        public async Task ConfirmEmail(ma01user ma01user, string code)
        {
            var result = await _userManager.ConfirmEmailAsync(ma01user, code);
            if (!result.Succeeded)
            {
                throw new Exception("Confirmação Falhou");
            }
            ma01user.EmailConfirmed = true;
            await _userManager.UpdateAsync(ma01user);
        }

        public async Task ResetPassword(ma01user ma01user, string code, string Password)
        {
            var result = await _userManager.ResetPasswordAsync(ma01user, code, Password);
            if (!result.Succeeded)
            {
                throw new Exception("Troca de senha falhou");
            }
        }
        public async Task<ma01user> Login(string Email, string Password)
        {
            ma01user user = await _userManager.FindByEmailAsync(Email);
            if (await _userManager.CheckPasswordAsync(user, Password) && await _userManager.IsEmailConfirmedAsync(user))
            {
                return user;
            }
            else if (await _userManager.CheckPasswordAsync(user, Password) && !await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new Exception("Usuário não confirmado. Favor, confirmar no seu e-mail!");
            }
            else if (!await _userManager.CheckPasswordAsync(user, Password) || user == null)
            {
                throw new Exception("Email ou senha inválidos!");
            }
            else
            {
                throw new Exception("Ocorreu um erro!");
            }
        }

        public async Task<string> GetPasswordResetToken(ma01user ma01user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(ma01user);
        }


        public async Task<ma01user> GetByIdAsync(int id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<ma01user> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task UpdateUser(ma01user ma01user)
        {
            await _userManager.UpdateAsync(ma01user);
        }
        public async Task ChangePassword(ma01user ma01user, string password, string newPassword)
        {
            try
            {
                await _userManager.ChangePasswordAsync(ma01user, password, newPassword);
            }
            catch (Exception)
            {
                throw new Exception("Senhas inseridas incorretamente!");
            }
        }
    }
}
