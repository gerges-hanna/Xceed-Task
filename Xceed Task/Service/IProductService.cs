using Microsoft.AspNetCore.Mvc.Rendering;
using Xceed_Task.Models;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync(int? categoryId);
    Task<List<Product>> GetPublicProductsAsync(int? categoryId);
    Task<Product> GetProductDetailsAsync(int id);
    Task<Product> GetProductByIdAsync(int id);
    Task CreateProductAsync(Product product, string userId);
    Task<bool> UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<SelectList> GetCategoriesSelectListAsync(int? selectedId = null);
}
