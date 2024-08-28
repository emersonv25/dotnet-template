using Loja.Api.Repositories;
using Loja.Api.Services.Interface;
using System.Security.Claims;

namespace Loja.Api.Services
{
    public class AuthService : IAuthService
    {
        public string GetFirebaseUidFromIdentity(ClaimsIdentity claimsIdentity)
        {
            string userId = TokenValidade(claimsIdentity);
            return userId;
        }

        private string TokenValidade(ClaimsIdentity claimsIdentity)
        {
            return claimsIdentity.Claims.ToList().FirstOrDefault(x => x.Type == "user_id").Value;
        }
    }
}
