using Microsoft.AspNetCore.Mvc;
using Template.Api.Models;
using Template.Domain.Entities;

namespace Template.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected User CurrentUser
        {
            get
            {
                return HttpContext.Items["User"] as User;
            }
        }

        protected string FirebaseId
        {
            get
            {
                return HttpContext.Items["FirebaseId"] as string;
            }
        }
    }
}
