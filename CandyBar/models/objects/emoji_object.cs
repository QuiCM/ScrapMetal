using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects
{
    [SuppressMessage("csharp", "IDE1006")]
    public class emoji_object
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<role_object> roles { get; set; }
        public user_object user { get; set; }
        public bool require_colons { get; set; }
        public bool managed { get; set; }
        public bool animated { get; set; }
        public bool available { get; set; }
    }
}