using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShopingList.Data;
using ShopingList.Data.Models;
using ShopingList.Features.Products.Services;

namespace ShopingList.Features.Products
{
    public class ProductService : IProductService, ICategoryService
    {
        private readonly ShopingListDBContext context;

        public ProductService(ShopingListDBContext context) => this.context = context;

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ProductCategory> CreateProductCategoryAsync(ProductCategory category)
        {
            try
            {
                context.ProductCategories.Add(category);
                await context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProductCategory>> GetAllProductCategoriesAsync()
        {
            try
            {
                return await context.ProductCategories.OrderBy(c => c.Name).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await context.Products.Include(p => p.Category).OrderBy(c => c.Category.Name).ThenBy(p => p.Name).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByPrefixAsync(string prefix)
        {
            try
            {
                return await context.Products.Where(p => p.Name.ToLower().Contains(prefix.ToLower())).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            try
            {
                return await context.Products.Include(p => p.Category).Where(x => x.Id == productId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Product> GetProductByNameAsync(string productName)
        {
            try
            {

                return await context.Products.Include(p => p.Category).Where(x => x.Name.ToLower().Equals(productName.ToLower())).FirstOrDefaultAsync(); ;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ProductCategory> GetProductCategoryByIdAsync(int categoryId)
        {
            try
            {
                return await context.ProductCategories.Where(x => x.Id == categoryId).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                context.Products.Update(product);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task UpdateProductCategoryAsync(ProductCategory category)
        {
            try
            {
                context.ProductCategories.Update(category);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteProductAsync(Product product)
        {
            try
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteProductCategoryAsync(ProductCategory category)
        {
            try
            {
                context.ProductCategories.Remove(category);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> CheckCategoryCanBeDeletedAsync(int categoryId)
        {
            try
            {
                return await context.Products.AnyAsync(p => p.CategoryId == categoryId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
