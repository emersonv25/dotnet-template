using Template.Domain.Entities;

namespace Ecommerce.API.Models.Dtos
{
    public class UserDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
    public class UserUpdateDto
    {
        public string Name { get; set; }
    }

    public class CreateUser : User
    {
        public CreateUser(UserDto dto, string firebaseId) : base(dto.Name, dto.Email, firebaseId)
        {
        }
    }
}
