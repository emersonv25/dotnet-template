using Template.Domain.Entities;
using Template.Domain.Interfaces;
using Template.Application.Interfaces;
using System.Threading.Tasks;
using Template.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Template.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByFirebaseIdAsync(string firebaseId)
        {
            return await _userRepository.GetByFirebaseIdAsync(firebaseId);
        }
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
        public async Task<User> CreateOrUpdateUser(UserRequestDto userDto, string firebaseId)
        {
            var existUser = await _userRepository.GetByFirebaseIdAsync(firebaseId);
            if (existUser != null)
            {
                existUser.UpdateName(userDto.Name);
                await _userRepository.UpdateAsync(existUser, existUser.Id);
                return existUser;
            }

            var newUser = new CreateUser(userDto, firebaseId);
            await _userRepository.AddAsync(newUser, null);
            return newUser;
        }
    }
}
