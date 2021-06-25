using System.Diagnostics.CodeAnalysis;
using CandyBar.models.enums;

namespace CandyBar.models.objects
{
    [SuppressMessage("csharp", "IDE1006")]
    public class interaction_data
    {
        public string custom_id { get; set; }
        public component_type component_type { get; set; }
    }
}