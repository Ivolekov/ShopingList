using ShopingList.Data.Models;

namespace ShopingList.Features.Products.Services
{
    public interface ICategoryService
    {
        Task<ProductCategory> CreateProductCategoryAsync(ProductCategory category);
        Task<IEnumerable<ProductCategory>> GetAllProductCategoriesAsync();
        Task<ProductCategory> GetProductCategoryByIdAsync(int categoryId);
        Task UpdateProductCategoryAsync(ProductCategory category);
        Task DeleteProductCategoryAsync(ProductCategory category);
        Task<bool> CheckCategoryCanBeDeletedAsync(int categoryId);
    }
}
