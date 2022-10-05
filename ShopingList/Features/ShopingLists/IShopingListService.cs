using ShopingList.Data.Models;

namespace ShopingList.Features.ShopingLists
{
    public interface IShopingListService
    {
        Task<ICollection<GroceryList>> GetAllGroceriesListAsync(string userId);
        Task<GroceryList> CreateGroceriesListAsync(GroceryList groceriesList);
        Task<GroceryList> GetGroceriesListByIdAsync(int Id);
        Task UpdateGroceriesListAsync(GroceryList groceriesList);
        Task DeleteGroceriesListAsync(GroceryList groceriesList);
        Task<Product_GroceryList> InsertPrductGroceryListAsync(Product_GroceryList productGroceryList);
        Task UpdateProductCroceryListAsync(Product_GroceryList productGroceryList);
        Task<Product_GroceryList> GetProductGroceryListByIdAsync(int productGroceryListId);
        Task DeleteProductCroceryListAsync(Product_GroceryList productGroceryList);
        Task<List<Product_GroceryList>> GetProductGroceryListByGLIdAsync(int groceryListId);
    }
}
