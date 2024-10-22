namespace Application.Posts.PostServices.Dto
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public int TimeRequired { get; set; }
        public string PostDescription { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
    }
}
