using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Api.DTOs;
using Template.Application.DTOs;
using Template.Application.Interfaces;

namespace Template.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserProfile(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }

        [HttpGet("firebase/{firebaseid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserFirebaseProfile(string firebaseid)
        {
            var user = await _userService.GetUserByFirebaseIdAsync(firebaseid);
            if(user is null)
            {
                return NotFound("Usuário não encontrado");
            }
            return Ok(user);
        }


        [HttpGet("LoggedUser")]
        public IActionResult GetLoggedUser()
        {
            var user = CurrentUser;
            return Ok(user);
        }

        [HttpGet("All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParamsDTO paginationParams)
        {
            var result = await _userService.GetAllAsync(paginationParams.PageNumber, paginationParams.PageSize);
            return Ok(result); 
        }

        [HttpPut("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] UserDTO userDTO)
        {
            if (FirebaseId == null)
            {
                throw new UnauthorizedAccessException("Acesso Negado: Usuário não logado");
            }

            var createdUser = await _userService.CreateOrUpdateUser(userDTO, FirebaseId);
            return Ok(createdUser);
        }

        // Outros métodos ainda não implementados
    }
}
