using ShopingList.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopingList.Data.Models
{
    public class GroceriesList
    {
        public GroceriesList() => ProductList = new List<Product>();

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<Product> ProductList { get; set; }
    }
}
