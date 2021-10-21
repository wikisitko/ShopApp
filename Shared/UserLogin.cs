using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Shared
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Proszę podaj email")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
