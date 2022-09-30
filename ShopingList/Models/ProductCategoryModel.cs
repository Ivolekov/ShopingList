using System.ComponentModel.DataAnnotations;

namespace ShopingList.Models
{
    public class ProductCategoryModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The category name should be less than 100 symbols!")]
        public string Name { get; set; } = string.Empty;
    }
}
