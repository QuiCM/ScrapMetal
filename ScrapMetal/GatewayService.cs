using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CandyBar;
using CandyBar.models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpeakEasy;

namespace ScrapMetal
{
    public class GatewayService : BackgroundService
    {
        private readonly ILogger<GatewayService> _logger;
        private readonly SpeakEasySocketService _wsService;
        private readonly DiscordApiService _apiService;
        private readonly HeartbeatService _heartbeatService;
        private readonly DiscordOptions _options;
        private CancellationToken _cancellation;
        private int? _sequence;
        private string _session;

        public GatewayService(ILogger<GatewayService> logger, SpeakEasySocketService webSocketService,
            DiscordApiService apiService, HeartbeatService heartbeatService,
            IOptions<DiscordOptions> discordOptions)
        {
            _logger = logger;
            _wsService = webSocketService;
            _apiService = apiService;
            _heartbeatService = heartbeatService;
            _options = discordOptions.Value ?? throw new Exception("Options borked"); //TODO: throw proper exception
        }

        protected override async Task ExecuteAsync(CancellationToken cancellation)
        {
            _cancellation = cancellation;

            _wsService.ConnectionUri = await _apiService.GetGatewayUriAsync();
            _wsService.OnMessageReceived += WsMessageReceivedHandler;

            await _wsService.StartAsync(cancellation);
        }

        private async void WsMessageReceivedHandler(object sender, Message message)
        {
            gateway_payload payload = JsonSerializer.Deserialize<gateway_payload>(message.Bytes);
            _logger.LogInformation($"Payload received: [{ payload.s?.ToString() ?? "-"}] { payload.op} | { payload.t ?? "NO EVENT"} | { payload.d}");
            _sequence = payload.s;

            switch (payload.op)
            {
                case 10:
                    StartHeartbeat(payload);
                    await ConnectToGatewayAsync();
                    break;

                case 9:
                    await ConnectToGatewayAsync(reidentify: true);
                    break;

                case 7:
                    await ReconnectToGateway();
                    break;

                case 0:
                    if (payload.t == "READY")
                    {
                        ProcessReadyEvent(payload);
                    }
                    else
                    {
                        DispatchGatewayEvent(payload);
                    }
                    break;
            }
        }

        private void StartHeartbeat(gateway_payload payload)
        {
            _heartbeatService.SetHeartbeatInterval(payload.d.GetProperty("heartbeat_interval").GetInt32());
            _heartbeatService.StartAsync(_cancellation);
        }

        private void ProcessReadyEvent(gateway_payload payload)
        {
            _session = payload.d.GetProperty("session_id").GetString();
        }

        private void DispatchGatewayEvent(gateway_payload payload)
        {
            //SynapseService.Dispatch(payload);
        }

        private async Task ReconnectToGateway()
        {
            //Stop the heartbeat - it will get started again when we reconnect and receive an op 10
            await _heartbeatService.StopAsync(_cancellation);
            //Stop the websocket, then start it again
            await _wsService.StopAsync(_cancellation);
            await _wsService.StartAsync(_cancellation);
        }

        private async Task ConnectToGatewayAsync(bool reidentify = false)
        {
            gateway_identity BuildIdentity()
            {
                return new()
                {
                    token = _options.ApiToken,
                    intents = (int)(gateway_intent_bitflags.GUILDS | gateway_intent_bitflags.GUILD_MEMBERS | gateway_intent_bitflags.GUILD_MESSAGES | gateway_intent_bitflags.GUILD_MESSAGE_REACTIONS | gateway_intent_bitflags.GUILD_MESSAGE_TYPING),
                    properties = new System.Collections.Generic.Dictionary<string, string> {
                        { "$os", Environment.OSVersion.Platform.ToString() },
                        { "$browser", "ScrapMetal" },
                        { "$device", "ScrapMetal" }
                    }
                };
            }

            async Task SendIdentifyAsync()
            {
                await _wsService.SendAsync(
                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new gateway_payload
                    {
                        op = 2,
                        d = BuildIdentity()
                    })),
                    System.Net.WebSockets.WebSocketMessageType.Text,
                    _cancellation
                );
            }

            async Task SendResumeAsync()
            {
                await _wsService.SendAsync(
                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new gateway_payload
                    {
                        op = 6,
                        d = new
                        {
                            token = _options.ApiToken,
                            session_id = _session,
                            seq = _sequence
                        }
                    })),
                    System.Net.WebSockets.WebSocketMessageType.Text,
                    _cancellation
                );
            }

            if (reidentify || _session == null)
            {
                await SendIdentifyAsync();
            }
            else
            {
                await SendResumeAsync();
            }
        }

    }
}