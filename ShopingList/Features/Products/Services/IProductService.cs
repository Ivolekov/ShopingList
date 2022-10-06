using ShopingList.Data.Models;

namespace ShopingList.Features.Products.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByPrefixAsync(string prefix);
        Task<Product> GetProductByIdAsync(int productId);
        Task<Product> GetProductByNameAsync(string productName);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<bool> IsProductExistsAsync(Product product1);
    }
}
