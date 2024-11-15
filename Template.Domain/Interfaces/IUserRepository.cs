using Template.Domain.Entities;

namespace Template.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByFirebaseIdAsync(string firebaseId);
        Task UpdateAsync(User user, Guid updatedBy);
        Task AddAsync(User user, Guid? createdBy);
    }
}
