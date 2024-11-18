using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Template.Api.DTOs;
using Template.Api.Extension;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Application.Services;
using Template.Domain.Entities;
using Template.Domain.Interfaces;

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
            return Ok(user);
        }


        [HttpGet()]
        public IActionResult GetLoggedUser()
        {
            var user = CurrentUser;
            return Ok(user);
        }

        [HttpGet("Throw")]
        [AllowAnonymous]
        public IActionResult Throw()
        {
            throw new KeyNotFoundException("User not found.");
        }

        [HttpGet("All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery]PagionationParamsDTO pagionationParams)
        {
            var usersDTO = await _userService.GetAllAsync(pagionationParams.PageNumber, pagionationParams.PageSize);

            var result = new PaginationResultDTO<UserDTO>(usersDTO, usersDTO.CurrentPage, usersDTO.PageSize, usersDTO.TotalCount, usersDTO.TotalPages);
            
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            if (FirebaseId == null)
            {
                throw new UnauthorizedAccessException("Unathorized: User not logged");
            }

            var createdUser = await _userService.CreateOrUpdateUser(userDTO, FirebaseId);
            return Ok(createdUser);
        }

        // Outros métodos ainda não implementados
    }
}
