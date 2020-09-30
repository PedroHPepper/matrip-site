using System;

namespace Matrip.Domain.Models.AccountModels
{
    public class TokenDTO
    {
        public string userName { get; set; }
        public string userType { get; set; }
        public DateTime Birthday { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationRefreshToken { get; set; }
    }
}
