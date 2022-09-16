using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace client.Models
{
    public class Driver
    {
        [Id]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string RegNo { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Make { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; internal set; }
    }
}
