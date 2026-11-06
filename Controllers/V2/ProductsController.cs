using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs.V2;
using ProductApi.Models;
using ProductApi.Services;
namespace ProductApi.Controllers.V2;
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService service;
    public ProductsController(ProductService service)
    {
        this.service = service;
    }
    private static ProductDto ToDto(Product product) => new() { Id = product.Id, ProductName = product.Name, Price = product.Price, Category = product.Category, StockQuantity = product.StockQuantity };
    [HttpGet] public IActionResult GetAll(string? category, decimal? minPrice, decimal? maxPrice)
    {
        var items = service.GetAll().AsEnumerable();
        if (!string.IsNullOrWhiteSpace(category)) items = items.Where(product => product.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        if (minPrice.HasValue) items = items.Where(product => product.Price >= minPrice.Value);
        if (maxPrice.HasValue) items = items.Where(product => product.Price <= maxPrice.Value);
        return Ok(items.Select(ToDto));
    }
    [HttpGet("{id:int}")] public IActionResult Get(int id)
    {
        var product = service.Get(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(ToDto(product));
    }
    [HttpPost] public IActionResult Add(ProductDto input)
    {
        if (input.Price <= 0)
        {
            return BadRequest("Price must be positive");
        }
        if (string.IsNullOrWhiteSpace(input.ProductName))
        {
            return BadRequest("Product name is required");
        }
        var product = service.Add(new Product { Name = input.ProductName, Price = input.Price, Category = input.Category, StockQuantity = input.StockQuantity });
        return Created($"/api/v2/products/{product.Id}", ToDto(product));
    }
    [HttpPut("{id:int}")] public IActionResult Update(int id, ProductDto input)
    {
        if (input.Price <= 0)
        {
            return BadRequest("Price must be positive");
        }
        if (string.IsNullOrWhiteSpace(input.ProductName))
        {
            return BadRequest("Product name is required");
        }
        var product = service.Get(id);
        if (product == null)
        {
            return NotFound();
        }
        product.Name = input.ProductName;
        product.Price = input.Price;
        product.Category = input.Category;
        product.StockQuantity = input.StockQuantity;
        return Ok(ToDto(product));
    }
    [HttpDelete("{id:int}")] public IActionResult Delete(int id)
    {
        var product = service.Get(id);
        if (product == null)
        {
            return NotFound();
        }
        service.Delete(product);
        return NoContent();
    }
}
