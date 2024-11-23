using MassTransit;
using MicroServiceDemo.OrderService.Context;
using MicroServiceDemo.OrderService.Models.Entities;
using MicroServiceDemo.OrderService.Models.Enums;
using MicroServiceDemo.OrderService.Models.ViewModel;
using MicroServiceDemo.Shared.Event;
using MicroServiceDemo.Shared.Message;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceDemo.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderServiceDbContext _context;
        readonly IPublishEndpoint _publishEndpoint;
        public OrdersController(OrderServiceDbContext context,IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult>CreateOrder(CreateOrderVM createOrder)
        {
            Order order = new()
            {
                OrderId = Guid.NewGuid(),
                BuyerId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                OrderStatu = OrderStatus.Suspended
            };
            order.OrderItems = createOrder.OrderItem.Select(oi => new OrderItem
            {
                Quantity = oi.Quantity,
                Price = oi.Price,
                ProductId = oi.ProductId,
            }).ToList();
            order.TotalPrice = createOrder.OrderItem.Sum(x => (x.Price*x.Quantity));
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            OrderCreatedEvent orderCreated = new()
            {
                BuyerId = order.BuyerId,
                OrderId = order.OrderId,
                OrderItems = order.OrderItems.Select(x => new OrderItemMessage
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };

            await _publishEndpoint.Publish(orderCreated);

            return Ok();
        }
    }
}
