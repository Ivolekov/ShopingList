using ShopingList.Data.Models;

namespace ShopingList.Features.Products.Models
{
    public class PagedProductCategoryVM
    {
        public PagedProductCategoryVM()
        {
            this.Categories = new List<ProductCategory>();
        }
        public List<ProductCategory> Categories { get; set; }
        public int PageSize { get; set; }
        public int ItemsCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
