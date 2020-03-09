using System;

namespace ExampleFlow.Models
{
    public class UserRegistrationRequest
    {
        public Guid RequestId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string IpAddress { get; set; }
    }
}
