using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects.embeds
{

    [SuppressMessage("csharp", "IDE1006")]
    public class embed_media_object
    {
        public string url { get; set; }
        public string proxy_url { get; set; }
        public int? height { get; set; }
        public int? width { get; set; }
    }
}