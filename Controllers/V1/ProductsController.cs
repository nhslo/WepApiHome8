using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs.V1;
using ProductApi.Models;
using ProductApi.Services;
namespace ProductApi.Controllers.V1;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService service;
    public ProductsController(ProductService service)
    {
        this.service = service;
    }
    private static ProductDto ToDto(Product product) => new() { Id = product.Id, Name = product.Name, Price = product.Price };
    [HttpGet] public IActionResult GetAll() => Ok(service.GetAll().Select(ToDto));
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
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            return BadRequest("Name is required");
        }
        var product = service.Add(new Product { Name = input.Name, Price = input.Price, Category = "", StockQuantity = 0 });
        return Created($"/api/v1/products/{product.Id}", ToDto(product));
    }
}
