using Loja.Api.Data;
using Loja.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loja.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User?> GetUserByFirebaseUid(string firebaseUid)
        {
            return await _context.User.FirstOrDefaultAsync(x => x.FirebaseUid == firebaseUid);
        }

        public async Task<User> CreateUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            user.ModifiedAt = DateTime.Now;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteUser(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
    
}
