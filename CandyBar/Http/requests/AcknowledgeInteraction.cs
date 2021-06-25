using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CandyBar.models.objects;

namespace CandyBar
{

    public partial class Discord
    {
        public partial class Http
        {
            public async Task AcknowledgeInteraction(interaction_object interaction, CancellationToken token)
            {
                HttpRequestMessage request = new()
                {
                    Content = JsonContent.Create(new { type = "4", data = new { content = "Processing..." } }),
                    RequestUri = new Uri($"v8/interactions/{interaction.id}/{interaction.token}/callback", UriKind.Relative),
                    Method = HttpMethod.Post
                };

                await _client.SendAsync(request, token);
            }
        }
    }
}