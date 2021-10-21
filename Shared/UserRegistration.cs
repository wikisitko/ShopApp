using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Shared
{
    public class UserRegistration
    {
        [EmailAddress , Required(ErrorMessage = "Email wymagany")]
        public string Email { get; set; }
        [StringLength(10, ErrorMessage = "Username is too long"), Required]
        public string Username { get; set; }
        [Required, StringLength(10, MinimumLength = 4)]
        public string Password { get; set; }
        [Required, Compare("Password", ErrorMessage = "Passwords are not the same.")]
        public string PasswordConfirm { get; set; }
        public string Description { get; set; }
        [Range(0, 10000)]
        public int Money { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "You have to confirm the rules!")]
        public bool Confirmation { get; set; }
    }
}
