namespace ShopingList.Features.Products.Models
{
    public class PagedProductVM
    {
        public PagedProductVM()
        {
            this.Products = new List<ProductVM>();
        }
        public List<ProductVM> Products { get; set; }
        public int PageSize { get; set; }
        public int ItemsCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
