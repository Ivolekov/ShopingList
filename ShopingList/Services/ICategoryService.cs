using ShopingList.Data.Models;

namespace ShopingList.Services
{
    public interface ICategoryService
    {
        Task<ProductCategory> CreateProductCategory(ProductCategory category);
        Task<IEnumerable<ProductCategory>> GetAllProductCategories();
        Task<ProductCategory> GetProductCategoryById(int categoryId);
        Task UpdateProductCategory(ProductCategory category);
        Task DeleteProductCategory(ProductCategory category);
        Task<bool> CheckCategoryCanBeDeleted(int categoryId);
    }
}
