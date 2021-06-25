using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects.embeds
{

    [SuppressMessage("csharp", "IDE1006")]
    public class embed_footer_object
    {
        public string text { get; set; }
        public string icon_url { get; set; }
        public string proxy_icon_url { get; set; }
    }
}