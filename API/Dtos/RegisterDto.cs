using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*[0-9!@#$%^&*\\?\\+])(?!.*[()_\\-\\`\\/\"\'|\\[\\]}{:;'/>.<,])(?!.*\\s)(?!.*\\s).{8,20}$", ErrorMessage ="password must have one uppercase one lowercase one number and at least 6 characters")]
        public string Password { get; set; }

    }
}