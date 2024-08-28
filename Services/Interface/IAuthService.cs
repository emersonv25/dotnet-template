using System.Security.Claims;

namespace Template.Api.Services.Interface
{
    public interface IAuthService
    {
        string? GetFirebaseUidFromIdentity(ClaimsIdentity claimsIdentity);
    }
}
