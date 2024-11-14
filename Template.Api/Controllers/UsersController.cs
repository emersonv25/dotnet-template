using Ecommerce.API.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Template.Api.Models;
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
        private readonly IMemoryCache _cache;
        public UsersController(IUserService userService, IMemoryCache cache)
        {
            _userService = userService;
            _cache = cache;
        }

        [HttpGet("test-cache")]
        [AllowAnonymous]
        public IActionResult TestCache()
        {
            if(_cache.TryGetValue("testKey", out string cachedValue))
            {
                Ok(cachedValue);
            }
            _cache.Set("testKey", "testValue", TimeSpan.FromMinutes(1));
            return Ok("cache setado");
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

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var createdUser = await _userService.CreateUser(userDto, CurrentUser.FirebaseId);
            return new OkObjectResult(createdUser);
        }

        // Outros métodos ainda não implementados
    }
}
