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
            var groceriesList = await context.GroceryLists.Where(gl => gl.UserId == userId).OrderByDescending(gl => gl.TimeStamp).ToListAsync();
            foreach (var gl in groceriesList)
            {
                gl.Product_GroceryList = await this.GetProductGroceryListByGLId(gl.Id);
            }
            return groceriesList;
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
                var groceryList = await context.GroceryLists.FirstOrDefaultAsync(groceries => groceries.Id == Id);
                groceryList.Product_GroceryList = await this.GetProductGroceryListByGLId(Id);
                return groceryList;
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

        public async Task<List<Product_GroceryList>> GetProductGroceryListByGLId(int groceryListId)
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

        public async Task<Product_GroceryList> InsertPrductGroceryList(Product_GroceryList productGroceryList) 
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

        public async Task UpdateProductCroceryList(Product_GroceryList productGroceryList) 
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

        public async Task DeleteProductCroceryList(Product_GroceryList productGroceryList)
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

        public async Task<Product_GroceryList> GetProductGroceryListById(int productGroceryListId)
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
