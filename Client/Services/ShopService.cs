using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using System.Net.Http;
using System.Net.Http.Json;
using BlazorShoppingApp.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorShoppingApp.Client.Services
{
    public class ShopService : IShopService
    {
        private readonly IToastService toastService;
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authProvider;

        public List<Item> Items { get; set; } = new List<Item>();
        public List<OrderResult> Orders { get; set; }

        public List<OrderItem> OrderItems { get; } = new List<OrderItem>();

        public ShopService(IToastService toastService, HttpClient httpClient, AuthenticationStateProvider authProvider)
        {
            this.toastService = toastService;
            this.httpClient = httpClient;
            this.authProvider = authProvider;
        }

        public async Task LoadItemsAsync() //wysyla zapytanie do backendu o dostepne rzeczy
        {
            if(Items.Count == 0)
            {
                await authProvider.GetAuthenticationStateAsync();
                try
                {
                    Items = await httpClient.GetFromJsonAsync<List<Item>>("api/Items");
                }
                catch (HttpRequestException)
                {
                    await authProvider.GetAuthenticationStateAsync();
                }
            }
        }

        public async Task LoadOrdersAsync()
        {
            await authProvider.GetAuthenticationStateAsync();
            try
            {
                Orders = await httpClient.GetFromJsonAsync<List<OrderResult>>("api/Shop/orders");
            }
            catch(HttpRequestException)
            {
            
            }
           
        }

        public async Task DeleteOrder(int orderId)
        {
            await authProvider.GetAuthenticationStateAsync();
            try
            {
                var result = await httpClient.DeleteAsync($"api/Shop/order/{orderId}");
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await authProvider.GetAuthenticationStateAsync();
                    toastService.ShowSuccess("Order removed!", "Success");
                    await LoadOrdersAsync();
                }
                else
                {
                    toastService.ShowError(await result.Content.ReadAsStringAsync(), "Error");
                }
            }
            catch(HttpRequestException)
            {
              

            }
        }

        public void AddItemToBasket(Item item)
        {
            var order = OrderItems.FirstOrDefault(x => x.Item.Id == item.Id);
            if(order == null)
            {
                order = new OrderItem
                {
                    Count = 1,
                    Item = item,
                    PricePerEach = item.Price
                };
                OrderItems.Add(order);
            }
            else
            {
                order.Count++;
            }
        }

        public async Task SendOrder(OrderForm orderForm)
        {
            var orderRequest = new OrderRequest
            {
                OrderForm = orderForm,
                Items = OrderItems.ToDictionary(x => x.Item.Id, y => y.Count)
            };

            await authProvider.GetAuthenticationStateAsync();
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Shop/order", orderRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    toastService.ShowSuccess("Zamówiono!");
                    OrderItems.Clear();
                }
                else
                {
                    toastService.ShowError("Nie udało się zrealizowć zamówienia");
                }
            }
           catch(HttpRequestException)
            {
    

            }
        }

        public async Task<List<OrderItem>> GetOrderedItems(int orderId)
        {
            await authProvider.GetAuthenticationStateAsync();
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<OrderItem>>($"api/Shop/orderItems/{orderId}");
                return response;
            }
            catch (HttpRequestException)
            {
               
            }

            return new List<OrderItem>();
        }
    }
}
