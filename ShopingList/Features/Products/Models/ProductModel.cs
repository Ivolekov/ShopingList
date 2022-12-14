using System.ComponentModel.DataAnnotations;

namespace ShopingList.Features.Products.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The product name should be less than 100 symbols!")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000, ErrorMessage = "Please choose product category!")]
        public int CategoryId { get; set; }

    }
}
