using ShopingList.Data.Models;

namespace ShopingList.Services
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product product);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int productId);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Product product);
    }
}
