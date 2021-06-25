using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models
{
    [SuppressMessage("csharp", "IDE1006")]
    public class gateway_payload
    {
        public int? op { get; set; }
        public int? s { get; set; }
        public string t { get; set; }
        public dynamic d { get; set; }
    }
}