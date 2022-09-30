using ShopingList.Data.Models;

namespace ShopingList.Services
{
    public interface ICategoryService
    {
        Task<ProductCategory> CreateProductCategory(ProductCategory category);
        Task<IEnumerable<ProductCategory>> GetAllProductCategories();
        Task<ProductCategory> GetProductCategoryById(int categoryId);
        Task<ProductCategory> GetProductCategoryByName(string categoryName);
        Task UpdateProductCategory(ProductCategory category);
        Task DeleteProductCategoryById(int categoryId);
        Task DeleteProductCategory(ProductCategory category);
    }
}
