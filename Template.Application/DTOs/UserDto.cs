using System.ComponentModel.DataAnnotations;
using Template.Domain.Entities;

namespace Template.Application.DTOs
{
    public class UserRequestDto
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength (255)]
        public required string Email { get; set; }
    }

    public class UserResponseDto
    {

    }

    public class CreateUser : User
    {
        public CreateUser(UserRequestDto dto, string firebaseId) : base(dto.Name, dto.Email, firebaseId)
        {
        }
    }
}
