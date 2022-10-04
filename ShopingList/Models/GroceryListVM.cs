using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShopingList.Data.Models;

namespace ShopingList.Models
{
    public class GroceryListVM
    {
        public GroceryListVM() => this.Product_GroceryList = new List<Product_GroceryList>();
        
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Product_GroceryList> Product_GroceryList { get; set; }
    }
}
