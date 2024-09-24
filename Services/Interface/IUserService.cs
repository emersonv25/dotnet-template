using Template.API.Models.Dtos;
using Template.API.Models.Entities;

namespace Template.API.Services.Interface
{
    public interface IUserService
    {
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByFirebaseUid(string firebaseUid);
        Task<User> CreateUser(UserDto userDto, string firebaseUid);
        Task<User?> UpdateUser(UserUpdateDto userDto, string firebaseUid);
    }
}
