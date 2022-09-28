using System.ComponentModel.DataAnnotations;

namespace ShopingList.Areas.Identity.Data
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
