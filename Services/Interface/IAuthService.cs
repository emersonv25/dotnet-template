using System.Security.Claims;

namespace Loja.Api.Services.Interface
{
    public interface IAuthService
    {
        string GetFirebaseUidFromIdentity(ClaimsIdentity claimsIdentity);
    }
}
