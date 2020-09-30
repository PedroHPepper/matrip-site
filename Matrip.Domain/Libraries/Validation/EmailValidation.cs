using System;
using System.Collections.Generic;
using System.Text;

namespace Matrip.Domain.Libraries.Validation
{
    public class EmailValidation
    {
        /// <summary>
        /// Validação de Email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
