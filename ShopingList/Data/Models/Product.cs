using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopingList.Data.Models
{
    public class Product
    {
        public Product() => Product_GroceryList = new List<Product_GroceryList>();

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public ProductCategory Category { get; set; }

        public ICollection<Product_GroceryList> Product_GroceryList { get; set; }
    }
}
