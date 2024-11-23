namespace MicroServiceDemo.OrderService.Models.ViewModel
{
    public class CreateOrderVM
    {
        public Guid BuyerId { get; set; }
       public List<CreateOrderItemVM>OrderItem { get; set; }
    }
    public class CreateOrderItemVM
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
