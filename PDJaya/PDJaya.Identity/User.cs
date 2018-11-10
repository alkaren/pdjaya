using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDJaya.Identity
{
    
    public class User
    {
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
    }
}
