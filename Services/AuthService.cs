using Template.Api.Repositories;
using Template.Api.Services.Interface;
using System.Security.Claims;

namespace Template.Api.Services
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
