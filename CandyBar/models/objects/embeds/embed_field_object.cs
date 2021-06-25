using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects.embeds
{

    [SuppressMessage("csharp", "IDE1006")]
    public class embed_field_object
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool inline { get; set; }
    }
}