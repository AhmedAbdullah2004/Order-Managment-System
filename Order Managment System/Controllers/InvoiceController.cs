using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IRepository<Invoice> _invoiceRepo;

    public InvoiceController(IRepository<Invoice> invoiceRepo)
    {
        _invoiceRepo = invoiceRepo;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var invoices = await _invoiceRepo.GetAllAsync();
        return Ok(invoices);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var invoice = await _invoiceRepo.GetByIdAsync(id);
        if (invoice == null) return NotFound();
        return Ok(invoice);
    }
}
