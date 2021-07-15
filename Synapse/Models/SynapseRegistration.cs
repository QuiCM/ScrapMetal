using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Synapse.Models
{
    public class SynapseRegistration
    {
        [JsonPropertyName("baseUrl")]
        public string BaseUrl { get; set; }
        [JsonPropertyName("neurons")]
        public List<Neuron> Neurons { get; set; }
    }
}