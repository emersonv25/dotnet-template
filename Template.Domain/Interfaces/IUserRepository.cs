using Template.Domain.Entities;
using Template.Domain.Pagination;

namespace Template.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetAllAsync(int pageNumber, int pageSize);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByFirebaseIdAsync(string firebaseId);
        Task UpdateAsync(User user, Guid updatedBy);
        Task AddAsync(User user, Guid? createdBy);
    }
}
