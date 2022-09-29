using ShopingList.Data;
using ShopingList.Data.Models;

namespace ShopingList.Services
{
    public class ProductService : IProductService, ICategoryService
    {
        private readonly ShopingListDBContext context;
        public ProductService(ShopingListDBContext context) => this.context = context;

        public async Task<Product> CreateProduct(Product product)
        {
            try
            {
                context.Add(product);
                await context.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task<ProductCategory> CreateProductCategory(ProductCategory category)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductById(int productId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductCategoryById(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductCategory>> GetAllProductCategories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductById(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByName(string productName)
        {
            throw new NotImplementedException();
        }

        public Task<ProductCategory> GetProductCategoryById(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductCategory> GetProductCategoryByName(string categoryName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductCategory(ProductCategory category)
        {
            throw new NotImplementedException();
        }
    }
}
