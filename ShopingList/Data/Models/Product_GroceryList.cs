using System.ComponentModel.DataAnnotations;

namespace ShopingList.Data.Models
{
    public class Product_GroceryList
    {
        [Key]
        public int Id { get; set; }

        public Product Product { get; set; } = new Product();

        public int ProductId { get; set; }

        public GroceryList GroceriesList { get; set; } = new GroceryList();

        public int GroceriesListId { get; set; }

        public bool IsBought { get; set; }
    }
}
