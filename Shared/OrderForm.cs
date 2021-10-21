using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Shared
{
    public class OrderForm
    {
        [Required, StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }
        [Required, StringLength(200, MinimumLength = 2)]
        public string Surname { get; set; }
        [Required, StringLength(200, MinimumLength = 2)]
        public string Address { get; set; }
    }
}
