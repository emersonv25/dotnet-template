using Template.API.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Template.API.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string FirebaseUid { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }

        public User() { }
        public User(UserDto dto, string firebaseUid)
        {
            Name = dto.Name;
            Email = dto.Email;
            FirebaseUid = firebaseUid;
        }
    }
}
