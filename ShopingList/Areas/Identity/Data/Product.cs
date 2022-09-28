using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopingList.Areas.Identity.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public ProductCategory Category { get; set; }
    }
}
