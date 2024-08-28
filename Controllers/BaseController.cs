using Template.Api.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected User LoggedUser
        {
            get
            {
#pragma warning disable CS8603
                return HttpContext.Items["User"] as User;
            }
        }
        protected string FirebaseUid
        {
            get
            {
#pragma warning disable CS8603
                return HttpContext.Items["FirebaseUid"] as string;
            }
        }
    }
}
