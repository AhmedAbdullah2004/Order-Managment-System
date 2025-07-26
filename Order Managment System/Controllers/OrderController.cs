using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IRepository<Order> _orderRepo;

    public OrderController(IOrderService orderService, IRepository<Order> orderRepo)
    {
        _orderService = orderService;
        _orderRepo = orderRepo;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderRepo.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    [Authorize(Roles = "Customer,Admin")]
    public async Task<IActionResult> Create(Order order)
    {
        var createdOrder = await _orderService.CreateOrderAsync(order);
        return CreatedAtAction(nameof(GetById), new { id = createdOrder.OrderId }, createdOrder);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        await _orderService.UpdateOrderStatusAsync(id, status);
        return NoContent();
    }

    [HttpPost("{id}/invoice")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GenerateInvoice(int id)
    {
        var invoice = await _orderService.GenerateInvoiceAsync(id);
        return Ok(invoice);
    }
}
