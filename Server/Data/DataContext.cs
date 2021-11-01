using BlazorShoppingApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorShoppingApp.Server.Data
{
    //Repository pattern - to klasa ktora trzyma dane i udostepnia metody do manipulacji danymi
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
