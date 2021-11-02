using System;
using Microsoft.AspNetCore.Identity;

namespace Model
{

    public class User : IdentityUser
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Address { get; set; } = "";
    }
}