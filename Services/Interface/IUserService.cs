using Template.Api.Models.Dtos;
using Template.Api.Models.Entities;

namespace Template.Api.Services.Interface
{
    public interface IUserService
    {
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByFirebaseUid(string firebaseUid);
        Task<User> CreateUser(UserDto userDto, string firebaseUid);
        Task<User?> UpdateUser(UserUpdateDto userDto, string firebaseUid);
    }
}
