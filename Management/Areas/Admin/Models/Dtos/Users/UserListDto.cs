namespace Management.Areas.Admin.Models.Dtos.Users
{
    public class UserListDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
