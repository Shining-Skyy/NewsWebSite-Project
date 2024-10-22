using Microsoft.AspNetCore.Identity;

namespace Domain.Roles
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }
    }
}
