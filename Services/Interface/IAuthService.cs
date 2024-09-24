using System.Security.Claims;

namespace Template.API.Services.Interface
{
    public interface IAuthService
    {
        string? GetFirebaseUidFromIdentity(ClaimsIdentity claimsIdentity);
    }
}
