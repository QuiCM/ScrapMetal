
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models
{
    [SuppressMessage("csharp", "IDE1006")]
    public class gateway_identity
    {
        public string token { get; set; }
        public int intents { get; set; }
        public Dictionary<string, string> properties { get; set; } = new Dictionary<string, string>();
    }
}