using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Template.Api.DTOs;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

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
        [SwaggerOperation(Summary = "Retorna um usuário baseado no seu Id")]
        [SwaggerResponse(200, "Usuário encontrado", typeof(User))]
        public async Task<IActionResult> GetUserProfile(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }

        [HttpGet("firebase/{firebaseid}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna um usuário baseado no seu FirebaseID")]
        [SwaggerResponse(200, "Usuário encontrado", typeof(User))]
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
        [SwaggerOperation(Summary = "Retorna o usuário atual autenticado")]
        [SwaggerResponse(200, "Usuário atual autenticado", typeof(User))]
        public IActionResult GetLoggedUser()
        {
            var user = CurrentUser;
            return Ok(user);
        }


        [HttpGet("All")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna uma lista de usuários paginada")]
        [SwaggerResponse(200, "Uma lista de usuários retornado", typeof(PaginatedResponseDTO<UserDTO>))]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParamsDTO paginationParams)
        {
            var result = await _userService.GetAllAsync(paginationParams.PageNumber, paginationParams.PageSize);
            return Ok(result); 
        }

        [HttpPut("CreateOrUpdate")]
        [SwaggerOperation(Summary = "Cria ou atualiza um usuário")]
        [SwaggerResponse(200, "Usuário criado ou atualizado com sucesso", typeof(User))]
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] UserDTO userDTO)
        {
            if (FirebaseId == null)
            {
                throw new UnauthorizedAccessException("Acesso Negado: Usuário não logado");
            }

            var createdUser = await _userService.CreateOrUpdateUser(userDTO, FirebaseId);
            return Ok(createdUser);
        }
    }
}
