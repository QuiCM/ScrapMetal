using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models
{
    [SuppressMessage("csharp", "IDE1006")]
    public enum permission_bitflags : ulong
    {
        MANAGE_CHANNELS = 1 << 4,
        ADD_REACTIONS = 1 << 6,
        VIEW_CHANNEL = 1 << 10,
        SEND_MESSAGES = 1 << 11,
        EMBED_LINKS = 1 << 14,
        ATTACH_FILES = 1 << 15,
        READ_MESSAGE_HISTORY = 1 << 16,
        USE_EXTERNAL_EMOJIS = 1 << 18,
        USE_SLASH_COMMANDS = (ulong)1 << 31,
        USE_PUBLIC_THREADS = (ulong)1 << 35
    }
}
