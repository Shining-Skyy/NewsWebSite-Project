namespace Application.Posts.GetPostPDP.Dto
{
    public class PostPDPDto
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Content { get; set; }
        public int TimeRequired { get; set; }
        public string Type { get; set; }
        public string NameAuthor { get; set; }
        public List<string> Image { get; set; }
        public List<SimilarPostDto> similarPosts { get; set; }
    }
}
