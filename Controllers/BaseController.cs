using Loja.Api.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected User LoggedUser
        {
            get
            {
                if (HttpContext.Items.TryGetValue("User", out var user))
                {
                    return user as User;
                }
                return null;
            }
        }
        protected string FirebaseUid
        {
            get
            {
                if (HttpContext.Items.TryGetValue("FirebaseUid", out var firebaseUid))
                {
                    return firebaseUid as string;
                }
                return null;
            }

        }
    }
}
