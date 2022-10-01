using Microsoft.EntityFrameworkCore;
using ShopingList.Data;
using ShopingList.Data.Models;

namespace ShopingList.Services
{
    public class ShopingListService : IShopingListService
    {
        private readonly ShopingListDBContext context;

        public ShopingListService(ShopingListDBContext context) => this.context = context;

        public async Task<ICollection<GroceryList>> GetAllGroceriesList(string userId)
        {
            return await context.GroceryLists.Where(gl=>gl.UserId == userId).OrderByDescending(gl=>gl.TimeStamp).ToListAsync();
        }

        public async Task<GroceryList> CreateGroceriesList(GroceryList groceriesList)
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

        public async Task<GroceryList> GetGroceriesListById(int Id)
        {
            try
            {
                return await context.GroceryLists.FirstOrDefaultAsync(groceries => groceries.Id == Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateGroceriesList(GroceryList groceriesList)
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

        public async Task DeleteGroceriesList(GroceryList groceriesList)
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

        public async Task AddProductToList(Product product, int shopingListId)
        {
            try
            {
                var gl = await context.GroceryLists.FirstAsync(x=>x.Id == shopingListId);
                //gl.ProductList.Add(product);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
