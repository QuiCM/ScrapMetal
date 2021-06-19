using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CandyBar.models
{
    [SuppressMessage("csharp", "IDE1006")]
    public class session_start_limit
    {
        public int total { get; set; }
        public int remaining { get; set; }
        public long reset_after { get; set; }
        public int max_concurrency { get; set; }
    }
}