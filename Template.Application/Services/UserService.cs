using Template.Domain.Entities;
using Template.Domain.Interfaces;
using Template.Application.Interfaces;
using System.Threading.Tasks;
using Template.Application.DTOs;
using System.ComponentModel.DataAnnotations;
using Template.Domain.Pagination;
using Template.Infra.Data.Helpers;

namespace Template.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PaginatedResponseDTO<UserDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            // Obtém o PagedList<User> do repositório
            var pagedUsers = await _userRepository.GetAllAsync(pageNumber, pageSize);

            // Mapeia os itens para UserDTO
            var userDTOs = pagedUsers.Select(user => new UserDTO(user)).ToList();  // Acessando diretamente pagedUsers

            var result = new PaginatedResponseDTO<UserDTO>(
                userDTOs,
                pagedUsers.CurrentPage,
                pagedUsers.PageSize,
                pagedUsers.TotalCount,
                pagedUsers.TotalPages
            );

            return result;
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
                existUser.UpdateEmail(userDTO.Email);
                await _userRepository.UpdateAsync(existUser, existUser.Id);
                return existUser;
            }

            var newUser = userDTO.ToEntity(firebaseId);
            await _userRepository.AddAsync(newUser, null);
            return newUser;
        }
    }
}
