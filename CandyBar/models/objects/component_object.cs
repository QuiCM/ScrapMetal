using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CandyBar.models.enums;

namespace CandyBar.models.objects
{
    [SuppressMessage("csharp", "IDE1006")]
    public class component_object
    {
        public component_type type { get; set; }
        public button_style? style { get; set; }
        public string label { get; set; }
        public emoji_object emoji { get; set; }
        public string custom_id { get; set; }
        public string url { get; set; }
        public bool disabled { get; set; }
        public List<component_object> components { get; set; }
    }
}