using Template.API.Repositories;
using Template.API.Services.Interface;
using System.Security.Claims;

namespace Template.API.Services
{
    public class AuthService : IAuthService
    {
        public string? GetFirebaseUidFromIdentity(ClaimsIdentity claimsIdentity)
        {
            return TokenValidade(claimsIdentity);
        }

        private string? TokenValidade(ClaimsIdentity claimsIdentity)
        {
            var userId = claimsIdentity.Claims.ToList().FirstOrDefault(x => x.Type == "user_id");

            if (userId != null)
            {
                return userId.Value;
            }
            else
                return null;
        }
    }
}
