using Template.Domain.Entities;
using System.Threading.Tasks;
using Template.Application.DTOs;
using Template.Domain.Pagination;

namespace Template.Application.Interfaces
{
    public interface IUserService
    {
        Task<PagedList<UserDTO>> GetAllAsync(int pageNumber, int pageSize);
        Task<User?> GetUserByFirebaseIdAsync(string firebaseId);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User> CreateOrUpdateUser(UserDTO userDTO, string firebaseId);
    }
}
