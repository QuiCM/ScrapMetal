using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects
{
    [SuppressMessage("csharp", "IDE1006")]
    public class role_object
    {
        public string id { get; set; }
        public string name { get; set; }
        public int color { get; set; }
        public bool hoist { get; set; }
        public int position { get; set; }
        public string permissions { get; set; }
        public bool managed { get; set; }
        public bool mentionable { get; set; }
    }
}