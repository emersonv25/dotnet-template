using Microsoft.AspNetCore.Mvc;
using Template.Domain.Entities;

namespace Template.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected User? CurrentUser => HttpContext.Items["User"] as User;

        protected string? FirebaseId => HttpContext.Items["FirebaseId"] as string;
    }
}
