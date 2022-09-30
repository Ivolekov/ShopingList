using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopingList.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.ShopingLists = new List<ShopingList>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        //[Required]
        public ProductCategory Category { get; set; }

        public ICollection<ShopingList> ShopingLists { get; set; }
    }
}
