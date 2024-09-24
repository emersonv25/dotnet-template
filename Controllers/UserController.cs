using Template.API.Models.Dtos;
using Template.API.Models.Entities;
using Template.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Template.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : BaseController
    {

        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }


        [HttpGet]
        public IActionResult GetLoggedUser()
        {
            return Ok(LoggedUser);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var createdUser = await _userService.CreateUser(userDto, FirebaseUid);
            return new OkObjectResult(createdUser);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userDto)
        {

            var updatedUser = await _userService.UpdateUser(userDto, FirebaseUid);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

    }
}
