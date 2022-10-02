using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShopingList.Data.Models;

namespace ShopingList.Models
{
    public class GroceryListVM
    {
        public GroceryListVM()
        {
            this.ProductList = new List<Product>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Product> ProductList { get; set; }
    }
}
