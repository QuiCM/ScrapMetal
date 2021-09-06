using System.Diagnostics.CodeAnalysis;
using CandyBar.models;

namespace CandyBar.models
{
    /*
    Note: System.Text.Json does not support renaming classes in Json serialization.
    Because of this we'll just use the API naming conventions in classes, and suppress the IDE warnings that this creates
    */
    [SuppressMessage("csharp", "IDE1006")]
    public class get_gateway_bot_response
    {
        public string url { get; set; }
        public int shards { get; set; }
        public session_start_limit session_start_limit { get; set; }
    }
}