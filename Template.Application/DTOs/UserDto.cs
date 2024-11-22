using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Template.Domain.Entities;

namespace Template.Application.DTOs
{
    public class UserDTO
    {
        [Required]
        [StringLength(256, ErrorMessage = "Tamanho máximo de caracteres é 255")]
        [SwaggerSchema(Description = "Nome do usuário")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255, ErrorMessage = "Tamanho máximo de caracteres é 255")]
        [SwaggerSchema(Description = "Email do usuário")]
        public string Email { get; set; }

        public UserDTO() { }

        public UserDTO(string name, string email) 
        {
            Name = name;
            Email = email;
        }
        public UserDTO(User user)
        {
            Name = user.Name;
            Email = user.Email;
        }
    }

}
