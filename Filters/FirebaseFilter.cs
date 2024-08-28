
using Template.Api.Repositories;
using Template.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Template.Api.Filters
{
    public class FirebaseFilter : IAuthorizationFilter
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public FirebaseFilter(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null && claimsIdentity.IsAuthenticated)
            {
                string firebaseUid = _authService.GetFirebaseUidFromIdentity(claimsIdentity);

                if (string.IsNullOrEmpty(firebaseUid))
                {
                    context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                    return;
                }

                context.HttpContext.Items["FirebaseUid"] = firebaseUid;

                // Verifica se a requisição está sendo feita para a ação CreateUser
                var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                if (actionDescriptor != null && actionDescriptor.ActionName == "CreateUser")
                {
                    // Não faz a verificação do usuário nulo para a ação CreateUser
                    return;
                }

                var user = _userRepository.GetUserByFirebaseUid(firebaseUid).Result;
                if(user == null)
                {
                    context.Result = new Microsoft.AspNetCore.Mvc.ConflictResult();
                }

                context.HttpContext.Items["User"] = user;
            }
        }
    }
}
