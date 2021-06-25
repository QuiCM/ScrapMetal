using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects.embeds
{

    [SuppressMessage("csharp", "IDE1006")]
    public class embed_author_object
    {
        public string name { get; set; }
        public string url { get; set; }
        public string icon_url { get; set; }
        public string proxy_icon_url { get; set; }
    }
}