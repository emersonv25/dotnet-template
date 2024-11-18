using Microsoft.EntityFrameworkCore;
using Template.Data;
using Template.Domain.Entities;
using Template.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using Template.Domain.Pagination;
using Template.Infra.Data.Helpers;

namespace Template.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedList<User>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query =  _context.Users.Where(x => x.DeletedAt == null).OrderByDescending(x => x.CreatedAt).AsQueryable();
            return await PaginationHelper.CreateAsync(query, pageNumber, pageSize); ;
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByFirebaseIdAsync(string firebaseId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.FirebaseId == firebaseId);
        }

        public async Task AddAsync(User user, Guid? createdBy)
        {
            user.CreatedBy = createdBy;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User user, Guid updateBy)
        {
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdateBy = updateBy;

            _context.Users.Update(user);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
