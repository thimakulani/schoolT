using System;
using System.Collections.Generic;
using System.Text;

namespace client.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public User ApplicationUser { get; set; }
    }
}
