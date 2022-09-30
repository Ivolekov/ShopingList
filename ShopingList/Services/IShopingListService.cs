using ShopingList.Data.Models;

namespace ShopingList.Services
{
    public interface IShopingListService
    {
        Task<ICollection<GroceriesList>> GetAllShopingList();
        Task<GroceriesList> CreateGroceriesList(GroceriesList groceriesList);
        Task<GroceriesList> GetGroceriesListById(int Id);
        Task UpdateGroceriesList(GroceriesList groceriesList);
        Task DeleteGroceriesList(GroceriesList groceriesList);
    }
}
