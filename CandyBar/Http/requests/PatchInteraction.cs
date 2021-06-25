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
            public async Task PatchInteraction(interaction_object interaction, message_object message, CancellationToken token)
            {
                HttpRequestMessage request = new()
                {
                    Content = JsonContent.Create(message),
                    RequestUri = new Uri($"webhooks/{interaction.application_id}/{interaction.token}/messages/{interaction.message.id}", UriKind.Relative),
                    Method = HttpMethod.Patch
                };

                HttpResponseMessage response = await _client.SendAsync(request, token);
            }
        }
    }
}