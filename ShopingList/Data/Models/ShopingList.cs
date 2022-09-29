using ShopingList.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopingList.Data.Models
{
    public class ShopingList
    {
        public ShopingList()
        {
            ProductList = new List<Product>();
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<Product> ProductList { get; set; }
    }
}
