using Loja.Api.Models.Dtos;
using Loja.Api.Models.Entities;

namespace Loja.Api.Services.Interface
{
    public interface IUserService
    {
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByFirebaseUid(string firebaseUid);
        Task<User> CreateUser(UserDto userDto, string firebaseUid);
        Task<User?> UpdateUser(UserUpdateDto userDto, string firebaseUid);
    }
}
