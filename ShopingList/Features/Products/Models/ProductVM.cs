using ShopingList.Data.Models;

namespace ShopingList.Features.Products.Models
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }
        public string Category { get; set; }
    }
}
