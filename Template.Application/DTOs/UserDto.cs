﻿using System.ComponentModel.DataAnnotations;
using Template.Domain.Entities;

namespace Template.Application.DTOs
{
    public class UserDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Max lenght is 100")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength (255, ErrorMessage = "Max lenght is 255")]
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
