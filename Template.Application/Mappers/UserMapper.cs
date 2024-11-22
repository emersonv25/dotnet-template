using Template.Application.DTOs;
using Template.Domain.Entities;

namespace Template.Application.Mappers
{
    public static class UserMapper
    {
        public static User ToEntity(this UserDTO dto, string firebaseId)
        {
            return new User(dto.Name, dto.Email, firebaseId); ;
        }

        public static UserDTO ToDTO(this User entity)
        {
            return new UserDTO(entity.Name, entity.Email);
        }
    }
}
