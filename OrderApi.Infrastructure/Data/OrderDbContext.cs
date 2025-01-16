using Microsoft.EntityFrameworkCore;
using OrderApi.Domain.Entites;

namespace OrderApi.Infrastructure.Data
{
    public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
    }
}
