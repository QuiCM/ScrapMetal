using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CandyBar.models;

namespace CandyBar.http.workers
{
    public class GatewayWorker : HttpWorker
    {
        public GatewayWorker(HttpClient client) : base(client) { }

        public async Task<get_gateway_bot_response> GetGatewayBotResponseAsync(CancellationToken cancellation)
        {
            HttpResponseMessage response = await _client.GetAsync("gateway/bot?v=9&encoding=json", cancellation);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Failed to retrieve gateway information", inner: null, statusCode: response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<get_gateway_bot_response>(cancellationToken: cancellation);
        }

        public async Task<Uri> GetGatewayUri(CancellationToken cancellation)
        {
            get_gateway_bot_response response = await GetGatewayBotResponseAsync(cancellation);
            return new(response.url);
        }
    }
}