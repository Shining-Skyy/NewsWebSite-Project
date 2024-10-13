using Microsoft.AspNetCore.Mvc.Rendering;

namespace Management.Areas.Admin.Models.Dtos.Roles
{
    public class AddUserRoleDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
