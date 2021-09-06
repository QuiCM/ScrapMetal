using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CandyBar.models.objects;

namespace CandyBar.http.workers
{
    public class InteractionWorker : HttpWorker
    {
        public InteractionWorker(HttpClient client) : base(client) { }

        public async Task<HttpResponseMessage> AcknowledgeInteraction(interaction_object interaction, CancellationToken cancellation)
        {
            HttpRequestMessage request = new()
            {
                Content = JsonContent.Create(new { type = "4", data = new { content = "Processing..." } }),
                RequestUri = new Uri($"v8/interactions/{interaction.id}/{interaction.token}/callback", UriKind.Relative),
                Method = HttpMethod.Post
            };

            return await _client.SendAsync(request, cancellation);
        }

        public async Task<HttpResponseMessage> PatchInteraction(interaction_object interaction, message_object message, CancellationToken cancellation)
        {
            HttpRequestMessage request = new()
            {
                Content = JsonContent.Create(message),
                RequestUri = new Uri($"webhooks/{interaction.application_id}/{interaction.token}/messages/{interaction.message.id}", UriKind.Relative),
                Method = HttpMethod.Patch
            };

            return await _client.SendAsync(request, cancellation);
        }

        public async Task<HttpResponseMessage> DeleteInteraction(interaction_object interaction, CancellationToken cancellation)
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri($"webhooks/{interaction.application_id}/{interaction.token}/messages/@original", UriKind.Relative),
                Method = HttpMethod.Delete
            };

            return await _client.SendAsync(request, cancellation);
        }
    }
}