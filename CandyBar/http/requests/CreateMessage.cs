using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CandyBar.builders;
using CandyBar.models.objects;

namespace CandyBar
{
    public partial class Discord
    {
        public partial class Http
        {
            public async Task CreateMessage(Action<MessageBuilder> builderFunc, CancellationToken token)
            {
                MessageBuilder builder = MessageBuilder.Create();
                builderFunc(builder);
                message_object message = builder.Message;

                HttpRequestMessage request = new()
                {
                    Content = JsonContent.Create(message),
                    RequestUri = new Uri($"channels/{message.channel_id}/messages", UriKind.Relative),
                    Method = HttpMethod.Post
                };
                HttpResponseMessage response = await _client.SendAsync(request, token);
            }
        }
    }
}