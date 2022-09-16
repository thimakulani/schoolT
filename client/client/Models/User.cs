using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace client.Models
{
    public class User
    {
        [Id]
        public string Id { get; set; }
        [MapTo("Name")]
        public string FirstName { get; set; }
        [MapTo("Surname")]
        public string LastName { get; set; }
        [MapTo("Phone")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        [MapTo("Url")]
        public string ImageUrl { get; set; } 
    }
}
