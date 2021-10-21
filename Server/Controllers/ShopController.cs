using BlazorShoppingApp.Server.Data;
using BlazorShoppingApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Server.Controllers
{
    [Route("api/[controller]")] //link (nazwa ENDPOINTU) do naszego kontrollera
    [ApiController]
    [Authorize] //zeby uzyc tego controllera (strzelic na niego zapytanie) bedziemy musieli zautoryzowac sie tokenem
    public class ShopController : ControllerBase
    {
        private readonly DataContext dataContext;

        public ShopController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        //public List<Item> Items { get; set; } = new List<Item>()
        //{
        //    new Item { Id=1, Brand="Hyundai", Name="i30", Price=40000 },
        //    new Item { Id=2, Brand="BMW", Name="E36", Price=3000 },
        //    new Item { Id=3, Brand="Kia", Name="Stinger", Price=120000 },
        //};

       

        //GET - zapytanie o jakas tresc
        //POST - zapytanie ktora umozliwia dodanie jakiejs tresci na serwerze
        //PUT - zapytanie zeby zmienic jakas istniejaca tresc na serwerze
        //DELETE - kasowanko :)
        //CRUD - CREATE, READ, UPDATE, DELETE



        [HttpGet("orders")]
        public async Task<IActionResult> AllOrders()
        {
            var user = await GetUser();
            if (user == null)
            {
                return NotFound("user not found");
            }

            var orders = await dataContext.Orders.Where(x => x.User.Id == user.Id).ToListAsync();
            var response = new List<OrderResult>();
            foreach (var order in orders)
            {
                var priceTotal = await dataContext.OrderItems.Where(orderItem => orderItem.Order.Id == order.Id).SumAsync(x => x.PricePerEach * x.Count);
                response.Add(new OrderResult
                {
                    Id = order.Id,
                    Address = order.Address,
                    Date = order.Date,
                    Name = order.Name,
                    Surname = order.Surname,
                    PriceTotal = priceTotal
                });
            }
            return Ok(response);
        }

        [HttpGet("orderItems/{orderId}")]
        public async Task<IActionResult> GetOrderItems(int orderId)
        {
            var user = await GetUser();
            if (user == null)
            {
                return NotFound("user not found");
            }

            var order = await dataContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.User.Id == user.Id);
            if(order == null)
            {
                return NotFound("order not found");
            }

            return Ok(await dataContext.OrderItems
                .Include(x => x.Order)
                .Include(x => x.Item)
                .Where(x => x.Order.Id == order.Id)
                .Select(x => new OrderItem
                {
                    Id = x.Id,
                    Count = x.Count,
                    Item = x.Item,
                    PricePerEach = x.PricePerEach
                })
                .ToListAsync());
        }

        [HttpPost("order")]
        public async Task<IActionResult> AddOrder(OrderRequest request)
        {
            var user = await GetUser();
            if (user == null)
            {
                return NotFound("user not found");
            }

            if(!await ItemsExists(request.Items.Select(x => x.Value).ToList()))
            {
                return NotFound("Item not found");
            }

            var order = new Order
            {
                Name = request.OrderForm.Name,
                Surname = request.OrderForm.Surname,
                User = user,
                Address = request.OrderForm.Address,
            };
            await dataContext.Orders.AddAsync(order);

            foreach (var it in request.Items)
            {
                var item = await dataContext.Items.FirstOrDefaultAsync(x => x.Id == it.Key);
                var orderItem = new OrderItem
                {
                    Count = it.Value,
                    Item = item,
                    Order = order,
                    PricePerEach = item.Price
                };
                await dataContext.OrderItems.AddAsync(orderItem);
            }
            await dataContext.SaveChangesAsync();
            return Ok(order.Id);
        }

        private async Task<bool> ItemsExists(List<int> ids)
        {
            foreach (var id in ids)
            {
                if(!await dataContext.Items.AnyAsync(x => x.Id == id))
                {
                    return false;
                }
            }
            return true;
        }

        private async Task<User> GetUser()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        }

        [HttpGet("money")]
        public async Task<IActionResult> GetMoney()
        {
            var user = await GetUser();
            if (user == null)
            {
                return NotFound("user not found");
            }

            return Ok(user.Money);
        }

        [HttpDelete("order/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var user = await GetUser();
            if (user == null)
            {
                return NotFound("user not found");
            }

            var order = await dataContext.Orders.FirstOrDefaultAsync(x => x.Id == id && x.User.Id == user.Id);
            if(order == null)
            {
                return NotFound("order not found");
            }
            dataContext.Orders.Remove(order);
            await dataContext.SaveChangesAsync();
            return Ok("Removed");
        }
    }
}
