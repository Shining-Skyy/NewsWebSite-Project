namespace Application.UriComposer
{
    public class UriComposerService : IUriComposerService
    {
        // This method composes a complete image URI by appending the source path to a base URL.
        public string ComposeImageUri(string src)
        {
            // The base URL is set to "https://localhost:44357/"
            // The Replace method is used to convert backslashes in the source path to forward slashes.
            // This is necessary because URIs typically use forward slashes.
            return "https://localhost:44357/" + src.Replace("\\", "//");
        }
    }
}
