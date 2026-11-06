using System.ComponentModel.DataAnnotations;
namespace ProductApi.DTOs.V2;
public class ProductDto
{
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)] public string ProductName { get; set; } = "";
    public decimal Price { get; set; }
    public string Category { get; set; } = "";
    [Range(0, int.MaxValue)] public int StockQuantity { get; set; }
    public bool IsAvailable => StockQuantity > 0;
}
