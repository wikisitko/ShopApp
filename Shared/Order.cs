using BlazorShoppingApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Shared
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
    }
}
