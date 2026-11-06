using System.ComponentModel.DataAnnotations;
namespace ProductApi.DTOs.V1;
public class ProductDto
{
    public int Id { get; set; }
    [Required(AllowEmptyStrings = false)] public string Name { get; set; } = "";
    public decimal Price { get; set; }
}
