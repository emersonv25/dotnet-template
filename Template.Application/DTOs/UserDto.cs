using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Template.Domain.Entities;

namespace Template.Application.DTOs
{
    public class UserDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Tamanho máximo é 100")]
        [SwaggerSchema(Description = "Nome do usuário")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255, ErrorMessage = "Tamanho máximo é 255")]
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

        public User ToEntity(string firebaseId)
        {
            return new User(Name, Email, firebaseId);
        }
    }

}
