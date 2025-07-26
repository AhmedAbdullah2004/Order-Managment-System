public interface IOrderService
{
    Task<Order> CreateOrderAsync(Order order);
    Task UpdateOrderStatusAsync(int orderId, string status);
    Task<Invoice> GenerateInvoiceAsync(int orderId);
}
