using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models
{
    [SuppressMessage("csharp", "IDE1006")]
    public class gateway_heartbeat
    {
        public int op { get; } = 1;
        public int? d { get; set; }
    }
}