using System.ComponentModel.DataAnnotations;
using Template.Domain.Entities;

namespace Template.Application.DTOs
{
    public class UserDTO
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength (255)]
        public required string Email { get; set; }

        public UserDTO() { }
        public UserDTO(User user)
        {
            Name = user.Name;
            Email = user.Email;
        }
    }

    public class CreateUser : User
    {
        public CreateUser(UserDTO dto, string firebaseId) : base(dto.Name, dto.Email, firebaseId)
        {
        }
    }
}
