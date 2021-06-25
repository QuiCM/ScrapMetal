using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.enums
{
    [SuppressMessage("csharp", "IDE1006")]
    public static class payload_event_types
    {
        public const string MESSAGE_CREATE = "MESSAGE_CREATE";
        public const string INTERACTION_CREATE = "INTERACTION_CREATE";
    }
}