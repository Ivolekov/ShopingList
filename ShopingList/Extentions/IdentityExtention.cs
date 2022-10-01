using System.Security.Claims;

namespace ShopingList.Extentions
{
    public static class IdentityExtention
    {
        public static string GetId(this ClaimsPrincipal user)
           => user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        public static string GetUsername(this ClaimsPrincipal user)
            => user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    }
}
