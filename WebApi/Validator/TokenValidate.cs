using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebApi.Validator
{
    public class TokenValidate
    {
        private readonly UserManager<User> userManager;
        public TokenValidate(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Execute(TokenValidatedContext context)
        {
            // Cast the Principal's Identity to ClaimsIdentity to access claims
            var claimsidentity = context.Principal.Identity as ClaimsIdentity;

            // Check if the ClaimsIdentity is null or has no claims
            if (claimsidentity?.Claims == null || !claimsidentity.Claims.Any())
            {
                context.Fail("claims not found....");
                return;
            }

            // Retrieve the UserId claim from the claims
            var userId = claimsidentity.FindFirst("UserId").Value;

            // Asynchronously find the user by their UserId
            var user = userManager.FindByIdAsync(userId).Result;

            if (user.LockoutEnabled == false)
            {
                context.Fail("User account is locked");
                return;
            }
        }
    }
}
