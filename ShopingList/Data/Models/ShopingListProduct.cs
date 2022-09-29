using ShopingList.Data.Models;

namespace ShopingList.Models
{
    public class ShopingListProduct : Product
    {
        public bool IsBought { get; set; }
        public int Quantity { get; set; }
    }
}
