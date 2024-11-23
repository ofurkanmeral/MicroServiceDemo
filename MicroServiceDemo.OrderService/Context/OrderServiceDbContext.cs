using MicroServiceDemo.OrderService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.OrderService.Context
{
    public class OrderServiceDbContext:DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem>OrderItems { get; set; }


        
    }
}
