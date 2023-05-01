using System;

namespace LicenseFunctionApp.Models
{
    public class License
    {
        public string id { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public string Renter { get; set; }
    }
}
