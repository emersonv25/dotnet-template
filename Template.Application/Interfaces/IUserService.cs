using Ecommerce.API.Models.Dtos;
using Template.Domain.Entities;
using System.Threading.Tasks;

namespace Template.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByFirebaseIdAsync(string firebaseId);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User> CreateUser(UserDto userDto, string firebaseUid);
    }
}
