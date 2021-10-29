using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Shared
{
    public class Fault
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [StringLength(12, ErrorMessage = "Login jest zbyt długi"), Required]
        public string Username { get; set; }
        [Required, StringLength(200, MinimumLength = 10)]
        public string Description { get; set; }
        [Range(0, 5)]
        public int Priority { get; set; }
        public DateTime DateOfFault { get; set; }
        public int Type { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "Potwierdź swój wiek")]
        public bool Confirmation { get; set; }
    }
}
