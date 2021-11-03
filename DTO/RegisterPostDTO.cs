using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
     public class RegisterPostDTO
    {
        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }

        [Required]
        [EmailAddress]
        public String Email { get; set; }

        public String PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}