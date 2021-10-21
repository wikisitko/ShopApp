using BlazorShoppingApp.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Client.Services //Client??
{
    public interface IShopService
    {
        List<Item> Items { get; set; }
        List<OrderResult> Orders { get; set; }
        List<OrderItem> OrderItems { get; }
        Task LoadItemsAsync();
        Task LoadOrdersAsync();
        Task DeleteOrder(int orderId);
        void AddItemToBasket(Item item);
        Task SendOrder(OrderForm orderForm);
        Task<List<OrderItem>> GetOrderedItems(int orderId);
    }
}