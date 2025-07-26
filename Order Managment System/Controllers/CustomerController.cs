using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepo;

    public CustomerController(ICustomerRepository customerRepo)
    {
        _customerRepo = customerRepo;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerRepo.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Customer customer)
    {
        await _customerRepo.AddAsync(customer);
        await _customerRepo.SaveAsync();
        return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, customer);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Customer updatedCustomer)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        if (customer == null) return NotFound();

        customer.Name = updatedCustomer.Name;
        customer.Email = updatedCustomer.Email;

        _customerRepo.Update(customer);
        await _customerRepo.SaveAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        if (customer == null) return NotFound();

        _customerRepo.Delete(customer);
        await _customerRepo.SaveAsync();
        return NoContent();
    }
}
