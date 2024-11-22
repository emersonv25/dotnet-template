using System;
using System.Collections.Generic;

namespace Template.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string FirebaseId { get; set; }

        public User() { }
        public User(string name, string email, string firebaseId)
        {
            Name = name;
            Email = email;
            FirebaseId = firebaseId;
        }

        public void UpdateFirebaseId(string firebaseId)
        {
            FirebaseId = firebaseId;
        }

        public void UpdateUserInfo(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
