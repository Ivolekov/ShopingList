using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShopingList.Data.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<GroceryList> GroceryLists { get; } = new List<GroceryList>(); 
    }
}
