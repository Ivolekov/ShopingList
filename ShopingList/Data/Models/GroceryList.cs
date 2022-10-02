using ShopingList.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopingList.Data.Models
{
    public class GroceryList
    {
        public GroceryList() => Product_GroceryList = new List<Product_GroceryList>();

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public List<Product_GroceryList> Product_GroceryList { get; set; }
    }
}
