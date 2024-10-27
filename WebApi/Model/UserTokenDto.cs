namespace WebApi.Model
{
    public class UserTokenDto
    {
        public string Id { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public string UserId { get; set; }
        public Domain.Users.User User { get; set; }
    }
}
