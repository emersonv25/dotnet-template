using System;
using System.Collections.Generic;

namespace Template.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirebaseId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User(string name, string email, string firebaseId)
        {
            FirebaseId = firebaseId;
            Name = name;
            Email = email;
        }
    }
}
