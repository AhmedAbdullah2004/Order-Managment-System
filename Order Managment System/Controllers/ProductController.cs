using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IRepository<Product> _productRepo;

    public ProductController(IRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepo.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Product product)
    {
        await _productRepo.AddAsync(product);
        await _productRepo.SaveAsync();
        return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Product updatedProduct)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null) return NotFound();

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Stock = updatedProduct.Stock;

        _productRepo.Update(product);
        await _productRepo.SaveAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null) return NotFound();

        _productRepo.Delete(product);
        await _productRepo.SaveAsync();
        return NoContent();
    }
}
