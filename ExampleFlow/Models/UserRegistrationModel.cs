using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleFlow.Models
{
    public class UserRegistrationModel
    {
        public Guid RequestId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Timezone { get; set; }
    }
}
