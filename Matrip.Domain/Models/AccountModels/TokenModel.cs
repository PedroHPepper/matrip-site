using System;

namespace Matrip.Domain.Models.AccountModels
{
    public class TokenModel
    {
        public string userName { get; set; }
        public string userType { get; set; }
        public DateTime birthday { get; set; }
        public string token { get; set; }
        public DateTime expiration { get; set; }
        public string refreshToken { get; set; }
        public DateTime expirationRefreshToken { get; set; }
    }
}
