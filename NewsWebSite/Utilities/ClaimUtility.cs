using System.Security.Claims;

namespace NewsWebSite.Utilities
{
    public static class ClaimUtility
    {
        // This method retrieves the user ID from the ClaimsPrincipal object
        public static string GetUserId(ClaimsPrincipal User)
        {
            // Cast the User's Identity to ClaimsIdentity to access claims
            var claimsIdentity = User.Identity as ClaimsIdentity;

            // Find the first claim of type NameIdentifier, which typically contains the user ID
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            return userId;
        }
    }
}
