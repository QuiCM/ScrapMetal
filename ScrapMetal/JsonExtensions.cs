using System.Buffers;
using System.Text.Json;

namespace ScrapMetal
{
    public static partial class JsonExtensions
    {
        public static T ToObject<T>(this JsonElement element, JsonSerializerOptions serializerOptions = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                element.WriteTo(writer);
            }
            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, serializerOptions);
        }
    }
}