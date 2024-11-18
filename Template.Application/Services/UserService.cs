using Template.Domain.Entities;
using Template.Domain.Interfaces;
using Template.Application.Interfaces;
using System.Threading.Tasks;
using Template.Application.DTOs;
using System.ComponentModel.DataAnnotations;
using Template.Domain.Pagination;
using AutoMapper;

namespace Template.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<UserDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var users = await _userRepository.GetAllAsync(pageNumber, pageSize);
            var dto = _mapper.Map<IEnumerable<UserDTO>>(users);
            return new PagedList<UserDTO>(dto, pageNumber, pageSize, users.Count);
        }

        public async Task<User?> GetUserByFirebaseIdAsync(string firebaseId)
        {
            return await _userRepository.GetByFirebaseIdAsync(firebaseId);
        }
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user;
        }
        public async Task<User> CreateOrUpdateUser(UserDTO userDTO, string firebaseId)
        {
            var existUser = await _userRepository.GetByFirebaseIdAsync(firebaseId);
            if (existUser != null)
            {
                existUser.UpdateName(userDTO.Name);
                await _userRepository.UpdateAsync(existUser, existUser.Id);
                return existUser;
            }

            var newUser = new CreateUser(userDTO, firebaseId);
            await _userRepository.AddAsync(newUser, null);
            return newUser;
        }
    }
}
