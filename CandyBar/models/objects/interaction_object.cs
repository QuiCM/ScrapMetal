using System.Diagnostics.CodeAnalysis;
using CandyBar.models.enums;

namespace CandyBar.models.objects
{
    [SuppressMessage("csharp", "IDE1006")]
    public class interaction_object
    {
        public string id { get; set; }
        public string application_id { get; set; }
        public interaction_type type { get; set; }
        public interaction_data data { get; set; }
        public string guild_id { get; set; }
        public string channel_id { get; set; }
        public member_object member { get; set; }
        public user_object user { get; set; }
        public string token { get; set; }
        public int version { get; set; }
        public message_object message { get; set; }
    }
}