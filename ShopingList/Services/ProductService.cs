using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
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
                context.Products.Add(product);
                await context.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ProductCategory> CreateProductCategory(ProductCategory category)
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

        public async Task<IEnumerable<ProductCategory>> GetAllProductCategories()
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

        public async Task<IEnumerable<Product>> GetAllProducts()
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

        public async Task<IEnumerable<Product>> GetProductsByPrefix(string prefix)
        {
            try
            {
                return await context.Products.Where(p => p.Name.Contains(prefix)).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Product> GetProductById(int productId)
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

        public async Task<Product> GetProductByName(string productName)
        {
            try
            {
                return await context.Products.Include(p => p.Category).Where(x => x.Name == productName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ProductCategory> GetProductCategoryById(int categoryId)
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

        public async Task UpdateProduct(Product product)
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

        public async Task UpdateProductCategory(ProductCategory category)
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

        public async Task DeleteProduct(Product product)
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

        public async Task DeleteProductCategory(ProductCategory category)
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
    }
}
