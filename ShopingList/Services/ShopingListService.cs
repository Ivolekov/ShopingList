using Microsoft.EntityFrameworkCore;
using ShopingList.Data;
using ShopingList.Data.Models;

namespace ShopingList.Services
{
    public class ShopingListService : IShopingListService
    {
        private readonly ShopingListDBContext context;

        public ShopingListService(ShopingListDBContext context) => this.context = context;

        public async Task<ICollection<GroceriesList>> GetAllShopingList()
        {
            return await context.ShopingLists.ToListAsync();
        }

        public async Task<GroceriesList> CreateGroceriesList(GroceriesList groceriesList)
        {
            try
            {
                context.ShopingLists.Add(groceriesList);
                await context.SaveChangesAsync();
                return groceriesList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GroceriesList> GetGroceriesListById(int Id)
        {
            try
            {
                return await context.ShopingLists.FirstOrDefaultAsync(groceries => groceries.Id == Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateGroceriesList(GroceriesList groceriesList)
        {
            try
            {
                context.ShopingLists.Update(groceriesList);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteGroceriesList(GroceriesList groceriesList)
        {
            try
            {
                context.ShopingLists.Remove(groceriesList);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
