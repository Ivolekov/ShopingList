using Microsoft.EntityFrameworkCore;
using ShopingList.Data;
using ShopingList.Data.Models;

namespace ShopingList.Features.ShopingLists
{
    public class ShopingListService : IShopingListService
    {
        private readonly ShopingListDBContext context;

        public ShopingListService(ShopingListDBContext context) => this.context = context;

        public async Task<ICollection<GroceryList>> GetAllGroceriesListAsync(string userId)
        {
            var groceriesList = await context.GroceryLists.Where(gl => gl.UserId == userId).OrderByDescending(gl => gl.TimeStamp).ToListAsync();
            foreach (var gl in groceriesList)
            {
                gl.Product_GroceryList = await GetProductGroceryListByGLIdAsync(gl.Id);
            }
            return groceriesList;
        }

        public async Task<GroceryList> CreateGroceriesListAsync(GroceryList groceriesList)
        {
            try
            {
                context.GroceryLists.Add(groceriesList);
                await context.SaveChangesAsync();
                return groceriesList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GroceryList> GetGroceriesListByIdAsync(int Id)
        {
            try
            {
                var groceryList = await context.GroceryLists.FirstOrDefaultAsync(groceries => groceries.Id == Id);
                return groceryList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateGroceriesListAsync(GroceryList groceriesList)
        {
            try
            {
                context.GroceryLists.Update(groceriesList);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteGroceriesListAsync(GroceryList groceriesList)
        {
            try
            {
                context.GroceryLists.Remove(groceriesList);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<Product_GroceryList>> GetProductGroceryListByGLIdAsync(int groceryListId)
        {
            try
            {
                return await context.Product_GroceryLists.Include(pgl => pgl.Product).ThenInclude(x => x.Category).Where(pgl => pgl.GroceriesListId == groceryListId).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Product_GroceryList> InsertPrductGroceryListAsync(Product_GroceryList productGroceryList)
        {
            try
            {
                context.Product_GroceryLists.Add(productGroceryList);
                await context.SaveChangesAsync();
                return productGroceryList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task UpdateProductCroceryListAsync(Product_GroceryList productGroceryList)
        {
            try
            {
                context.Product_GroceryLists.Update(productGroceryList);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteProductCroceryListAsync(Product_GroceryList productGroceryList)
        {
            try
            {
                context.Product_GroceryLists.Remove(productGroceryList);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Product_GroceryList> GetProductGroceryListByIdAsync(int productGroceryListId)
        {
            try
            {
                var productGroceryList = await context.Product_GroceryLists.Include(pgl => pgl.Product).Include(pgl => pgl.GroceriesList).FirstOrDefaultAsync(pgl => pgl.Id == productGroceryListId);
                return productGroceryList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
