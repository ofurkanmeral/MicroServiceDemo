using MassTransit;
using MicroServiceDemo.Shared.Event;

namespace MicroServiceDemo.StockService.Consumer
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            Console.WriteLine(context.Message.OrderId + "Eventi Yakalandı");
            return Task.CompletedTask;
        }
    }
}
