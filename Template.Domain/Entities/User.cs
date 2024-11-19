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

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome não pode ser vazio", nameof(name));

            Name = name;
        }

        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                throw new ArgumentException("Email inválidop.", nameof(email));

            Email = email;
        }

        public void UpdateFirebaseId(string firebaseId)
        {
            if (string.IsNullOrWhiteSpace(firebaseId))
                throw new ArgumentException("FirebaseId não pode ser vazio.", nameof(firebaseId));

            FirebaseId = firebaseId;
        }

        public void UpdateUserInfo(string name, string email, string firebaseId)
        {
            UpdateName(name);
            UpdateEmail(email);
            UpdateFirebaseId(firebaseId);
        }
        private bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains("."); // Exemplo simplificado de validação
        }
    }
}
