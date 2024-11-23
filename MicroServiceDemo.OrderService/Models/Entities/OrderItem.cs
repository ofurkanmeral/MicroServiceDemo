namespace MicroServiceDemo.OrderService.Models.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } = decimal.Zero;
        public Order Order { get; set; }
    }
}
