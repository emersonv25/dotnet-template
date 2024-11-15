using Template.Domain.Entities;
using System.Threading.Tasks;
using Template.Application.DTOs;

namespace Template.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByFirebaseIdAsync(string firebaseId);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User> CreateOrUpdateUser(UserRequestDto userDto, string firebaseId);
    }
}
