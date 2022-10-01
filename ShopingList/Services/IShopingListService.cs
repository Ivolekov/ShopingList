using ShopingList.Data.Models;

namespace ShopingList.Services
{
    public interface IShopingListService
    {
        Task<ICollection<GroceryList>> GetAllGroceriesList(string userId);
        Task<GroceryList> CreateGroceriesList(GroceryList groceriesList);
        Task<GroceryList> GetGroceriesListById(int Id);
        Task UpdateGroceriesList(GroceryList groceriesList);
        Task DeleteGroceriesList(GroceryList groceriesList);
        Task AddProductToList(Product product, int shopingListId);
    }
}
