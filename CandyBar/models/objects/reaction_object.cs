using System.Diagnostics.CodeAnalysis;

namespace CandyBar.models.objects
{
    [SuppressMessage("csharp", "IDE1006")]
    public class reaction_object
    {
        public int count { get; set; }
        public bool me { get; set; }
        public emoji_object emoji { get; set; }
    }
}