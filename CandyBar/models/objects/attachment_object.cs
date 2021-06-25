using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects
{

    [SuppressMessage("csharp", "IDE1006")]
    public class attachment_object
    {
        public string id { get; set; }
        public string filename { get; set; }
        public string content_type { get; set; }
        public int size { get; set; }
        public string url { get; set; }
        public string proxy_url { get; set; }
        public int? height { get; set; }
        public int? width { get; set; }
    }
}