public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepo;
    private readonly IRepository<Product> _productRepo;
    private readonly IRepository<OrderItem> _orderItemRepo;
    private readonly IRepository<Invoice> _invoiceRepo;

    public OrderService(
        IRepository<Order> orderRepo,
        IRepository<Product> productRepo,
        IRepository<OrderItem> orderItemRepo,
        IRepository<Invoice> invoiceRepo)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
        _orderItemRepo = orderItemRepo;
        _invoiceRepo = invoiceRepo;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
                foreach (var item in order.OrderItems)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            if (product.Stock < item.Quantity)
                throw new Exception($"Product {product.Name} is out of stock!");

       
            product.Stock -= item.Quantity;
            _productRepo.Update(product);
        }

        order.TotalAmount = order.OrderItems.Sum(i => i.UnitPrice * i.Quantity);

        ApplyDiscounts(order);

        await _orderRepo.AddAsync(order);
        await _orderRepo.SaveAsync();

        return order;
    }

    private void ApplyDiscounts(Order order)
    {
        if (order.TotalAmount > 200)
            order.TotalAmount *= 0.90m;
        else if (order.TotalAmount > 100)
            order.TotalAmount *= 0.95m; 
    }

    public async Task UpdateOrderStatusAsync(int orderId, string status)
    {
        var order = await _orderRepo.GetByIdAsync(orderId);
        if (order == null) throw new Exception("Order not found");

        order.Status = status;
        _orderRepo.Update(order);
        await _orderRepo.SaveAsync();
    }

    public async Task<Invoice> GenerateInvoiceAsync(int orderId)
    {
        var order = await _orderRepo.GetByIdAsync(orderId);
        if (order == null) throw new Exception("Order not found");

        var invoice = new Invoice
        {
            OrderId = order.OrderId,
            InvoiceDate = DateTime.Now,
            TotalAmount = order.TotalAmount
        };

        await _invoiceRepo.AddAsync(invoice);
        await _invoiceRepo.SaveAsync();

        return invoice;
    }
}
