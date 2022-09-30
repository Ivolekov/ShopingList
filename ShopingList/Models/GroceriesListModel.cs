using System.ComponentModel.DataAnnotations;

namespace ShopingList.Models
{
    public class GroceriesListModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The groceries list title should be less than 100 symbols!")]
        public string Title { get; set; } = string.Empty;
    }
}
