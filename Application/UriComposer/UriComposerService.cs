namespace Application.UriComposer
{
    public class UriComposerService : IUriComposerService
    {
        public string ComposeImageUri(string src)
        {
            return "https://localhost:44357/" + src.Replace("\\", "//");
        }
    }
}
