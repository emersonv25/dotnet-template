using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Api.Models
{
    public class FirebaseUser
    {
        public string Id { get; set; }
        public string Email { get; set; } 
        public string Name { get; set; }

        public FirebaseUser(string id, string email, string name)
        {
            Id = id;
            Email = email;
            Name = name;
        }
    }
}
