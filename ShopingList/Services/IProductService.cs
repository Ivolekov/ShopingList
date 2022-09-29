using ShopingList.Data.Models;

namespace ShopingList.Services
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product product);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int productId);
        Task<Product> GetProductByName(string productName);
        Task UpdateProduct(Product product);
        Task DeleteProductById(int productId);
    }
}
