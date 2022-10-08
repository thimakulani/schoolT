using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using BackendlessAPI;
using Weborb.Service;

namespace client.Models
{
    public class User
    {
 
        [SetClientClassMemberName("firstname")]
        public string FirstName { get; set; }
        [SetClientClassMemberName("lastname")]
        public string LastName { get; set; }
        [SetClientClassMemberName("phonenumber")]
        public string PhoneNumber { get; set; }
        [SetClientClassMemberName("role")]
        public string Role { get; set; }
        [SetClientClassMemberName("imageurl")]
        public string ImageUrl { get; set; } 
    }
}
