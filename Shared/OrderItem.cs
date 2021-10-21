using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Shared
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public Order Order { get; set; }
        public int Count { get; set; }
        public int PricePerEach { get; set; }
    }
}
