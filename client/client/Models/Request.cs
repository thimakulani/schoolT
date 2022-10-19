using Plugin.CloudFirestore.Attributes;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace client.Models
{
    public class Request
    {
        public string DriverId { get; set; }

        public string PickupAddress { get; set; }
        public double PickupLat { get; set; }
        public double PickupLong { get; set; }
        public string DestinationAddress { get; set; }
        public double DestinationLat { get; set; }
        public double DestinationLong { get; set; }
        public string RequestTime { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public double Distance { get; set; }
        public string EstimatedTime { get; set; } 
    }
}
