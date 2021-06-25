using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects
{
    [SuppressMessage("csharp", "IDE1006")]
    public class member_object
    {
        public user_object user { get; set; }
        public string nick { get; set; }
        public List<dynamic> roles { get; set; }
        public DateTime joined_at { get; set; }
        public bool deaf { get; set; }
        public bool mute { get; set; }
        public bool pending { get; set; }
        public string permissions { get; set; }
    }
}