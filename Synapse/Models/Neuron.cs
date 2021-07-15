using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Synapse.Models
{
    public class Neuron
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("events")]
        public List<string> Events { get; set; }
    }
}