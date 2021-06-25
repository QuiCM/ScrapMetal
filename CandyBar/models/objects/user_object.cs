using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects
{
    [SuppressMessage("csharp", "IDE1006")]
    public class user_object
    {
        public string id { get; set; }
        public string username { get; set; }
        public string discriminator { get; set; }
        public string avatar { get; set; }
        public bool bot { get; set; }
        public bool system { get; set; }
    }
}