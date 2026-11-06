using ProductApi.Models;
namespace ProductApi.Services;
public class ProductService
{
    private readonly List<Product> products = new()
    {
        new() { Id = 1, Name = "Laptop", Price = 350000, Category = "Electronics", StockQuantity = 15 },
        new() { Id = 2, Name = "Book", Price = 5000, Category = "Books", StockQuantity = 4 },
        new() { Id = 3, Name = "Phone", Price = 180000, Category = "Electronics", StockQuantity = 0 }
    };
    public List<Product> GetAll()
    {
        return products;
    }
    public Product? Get(int id) => products.FirstOrDefault(item => item.Id == id);
    public Product Add(Product product)
    {
        if (products.Count == 0)
        {
            product.Id = 1;
        }
        else
        {
            product.Id = products.Max(item => item.Id) + 1;
        }
        products.Add(product);
        return product;
    }
    public void Delete(Product product) => products.Remove(product);
}
