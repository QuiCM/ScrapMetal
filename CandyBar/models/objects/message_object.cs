using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CandyBar.models.enums;
using CandyBar.models.objects.embeds;

namespace CandyBar.models.objects
{
    [SuppressMessage("csharp", "IDE1006")]
    public class message_object
    {
        public string id { get; set; }
        public string channel_id { get; set; }
        public string guild_id { get; set; }
        public user_object author { get; set; }
        public member_object member { get; set; }
        public string content { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime? edited_timestamp { get; set; }
        public bool tts { get; set; }
        public bool mention_everyone { get; set; }
        public dynamic mentions { get; set; }
        public List<role_object> mention_roles { get; set; }
        public List<attachment_object> attachments { get; set; }
        public List<embed_object> embeds { get; set; }
        public List<reaction_object> reactions { get; set; }
        public bool pinned { get; set; }
        public string webhook_id { get; set; }
        public message_type type { get; set; } = message_type.DEFAULT;
        public List<component_object> components { get; set; }
    }
}