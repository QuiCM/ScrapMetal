using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.enums
{
    [SuppressMessage("csharp", "IDE1006")]
    public enum interaction_type
    {
        Ping = 1,
        ApplicationCommand = 2,
        MessageComponent = 3
    }
}