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
        Task<Product_GroceryList> InsertPrductGroceryList(Product_GroceryList productGroceryList);
        Task UpdateProductCroceryList(Product_GroceryList productGroceryList);
        Task<Product_GroceryList> GetProductGroceryListById(int productGroceryListId);
        Task DeleteProductCroceryList(Product_GroceryList productGroceryList);
        Task<List<Product_GroceryList>> GetProductGroceryListByGLId(int groceryListId);
    }
}
