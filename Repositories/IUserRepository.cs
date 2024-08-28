using Loja.Api.Models.Entities;

namespace Loja.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByFirebaseUid(string firebaseUid);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task<User?> DeleteUser(Guid id);
    }
}
