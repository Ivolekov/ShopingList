using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShopingList.Data.Models;

namespace ShopingList.Data
{
    public static class DataSeed
    {
        public static void Seed(this IHost host) 
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ShopingListDBContext>();
            context.Database.EnsureCreated();
            AddCategories(context);
            AddProducts(context);
            scope.Dispose();
            context.Dispose();
        }

        private static void AddCategories(ShopingListDBContext context)
        {
            var category = context.ProductCategories.FirstOrDefault();
            if (category != null) return;

            context.ProductCategories.Add(new ProductCategory 
            {
                Id = 1,
                Name = "Fruits"
            });

            context.ProductCategories.Add(new ProductCategory
            {
                Id = 2,
                Name = "Vegetables"
            });

            context.ProductCategories.Add(new ProductCategory
            {
                Id = 3,
                Name = "Meat"
            });

            context.ProductCategories.Add(new ProductCategory
            {
                Id = 4,
                Name = "Beverages"
            });

            context.ProductCategories.Add(new ProductCategory
            {
                Id = 5,
                Name = "Frozen Foods"
            });

            context.SaveChanges();
        }

        private static void AddProducts(ShopingListDBContext context)
        {
            var product = context.Products.FirstOrDefault();
            if (product != null) return;

            context.Products.Add(new Product 
            {
                Id = 1, Name = "Orange",
                CategoryId = 1
            });

            context.Products.Add(new Product
            {
                Id = 2, Name = "Apple",
                CategoryId = 1
            });

            context.Products.Add(new Product
            {
                Id = 3, Name = "Cuccumber",
                CategoryId = 2
            });

            context.Products.Add(new Product
            {
                Id = 4, Name = "Tomato",
                CategoryId = 2
            });

            context.Products.Add(new Product
            {
                Id = 5, Name = "Ice cream",
                CategoryId = 3
            });

            context.Products.Add(new Product
            {
                Id = 6, Name = "Pizza",
                CategoryId = 3              
            });

            context.Products.Add(new Product
            {
                Id = 7, Name = "Beef",
                CategoryId = 4
            });

            context.Products.Add(new Product
            {
                Id = 8, Name = "Pork",
                CategoryId = 4
            });

            context.Products.Add(new Product
            {
                Id = 9, Name = "Soda",
                CategoryId = 5
            });

            context.SaveChanges();
        }        
    }
}
