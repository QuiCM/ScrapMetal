using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CandyBar.responses;

namespace CandyBar
{
    public partial class Discord
    {
        public partial class Http
        {
            ///<summary>Retrieves a gateway connection url</summary>
            public async Task<string> GetGatewayUrl(CancellationToken token)
            {
                HttpResponseMessage response = await _client.GetAsync("gateway/bot?v=9&encoding=json", token);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Failed to retrieve gateway information", inner: null, statusCode: response.StatusCode);
                }

                get_gateway_bot_response gateway = await response.Content.ReadFromJsonAsync<get_gateway_bot_response>(cancellationToken: token);

                return gateway.url;
            }
        }
    }
}