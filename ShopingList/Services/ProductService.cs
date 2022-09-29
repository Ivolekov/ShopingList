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

        public async Task DeleteProductById(int productId)
        {
            try
            {
                var product = await this.GetProductById(productId);
                if (product == null)
                {
                    throw new Exception("");
                }
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteProductCategoryById(int categoryId)
        {
            try
            {
                var productCategory = await this.GetProductCategoryById(categoryId);
                if (productCategory == null)
                {
                    throw new Exception("");
                }
                context.ProductCategories.Remove(productCategory);
                await context.SaveChangesAsync();
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
                return await context.ProductCategories.OrderBy(c => c.Id).ToListAsync();
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
                return await context.Products.OrderBy(c => c.Id).ToListAsync();
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
                return await context.Products.Where(x => x.Id == productId).FirstOrDefaultAsync();
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
                return await context.Products.Where(x => x.Name == productName).FirstOrDefaultAsync();

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

        public async Task<ProductCategory> GetProductCategoryByName(string categoryName)
        {
            try
            {
                return await context.ProductCategories.Where(x => x.Name == categoryName).FirstOrDefaultAsync();

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
    }
}
