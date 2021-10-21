using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Shared
{
    public class OrderRequest
    {
        public Dictionary<int, int> Items { get; set; }
        public OrderForm OrderForm { get; set; }
    }
}
