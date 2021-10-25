using BlazorShoppingApp.Server.Data;
using BlazorShoppingApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly DataContext dataContext;

        public ItemsController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet] //zapytanie typu GET - sluzy do pobierania tresci z serwera
        public async Task<IActionResult> GetItems()
        {
            var items = await dataContext.Items.ToListAsync(); //w funkcji typu async jesli wywolujemy cos co tez jest async to trzeba dodac await przed wywolaniem
            return Ok(items); //OK - sukces, czyli przegladarka otrzyma kod 200
            //return NotFound(); //404 kod
            //return NoContent(); //402 kod
            //return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(Item item)
        {
            await dataContext.AddAsync(item);
            await dataContext.SaveChangesAsync();
            var list = await dataContext.Items.ToListAsync();
            return Ok(list);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            Item dbItem = await dataContext.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (dbItem == null)
            {
                return NotFound("item not found");
            }

            dbItem.Name = item.Name;
            dbItem.ImageName = item.ImageName;
            dbItem.Price = item.Price;
            await dataContext.SaveChangesAsync();
            var list = await dataContext.Items.ToListAsync();
            return Ok(list);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            Item dbItem = await dataContext.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (dbItem == null)
            {
                return NotFound("item not found");
            }

            dataContext.Remove(dbItem);
            await dataContext.SaveChangesAsync();
            var list = await dataContext.Items.ToListAsync();
            return Ok(list);
        }
    }
}
