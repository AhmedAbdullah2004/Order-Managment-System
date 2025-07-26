using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly OrderManagementDbContext _context;

        public OrderRepository(OrderManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
    }
}
