using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects.embeds
{

    [SuppressMessage("csharp", "IDE1006")]
    public class embed_object
    {
        public string title { get; set; }
        public string type { get; set; } = "rich";
        public string description { get; set; }
        public string url { get; set; }
        public DateTime? timestamp { get; set; }
        public int color { get; set; }
        public embed_footer_object footer { get; set; }
        public embed_media_object image { get; set; }
        public embed_media_object thumbnail { get; set; }
        public embed_media_object video { get; set; }
        public embed_provider_object provider { get; set; }
        public embed_author_object author { get; set; }
        public List<embed_field_object> fields { get; set; }
    }
}