using OrderManagementSystem.Models;

namespace OrderManagementSystem.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<Order?> GetOrderWithItemsAsync(int orderId);
    }
}
