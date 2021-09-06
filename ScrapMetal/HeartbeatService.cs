using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CandyBar.models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeakEasy;

namespace ScrapMetal
{
    public class HeartbeatService : BackgroundService
    {
        private readonly ILogger<HeartbeatService> _logger;
        private readonly SpeakEasySocketService _wsService;
        private int _interval;

        public HeartbeatService(ILogger<HeartbeatService> logger, SpeakEasySocketService wsService)
        {
            _logger = logger;
            _wsService = wsService;
        }

        public void SetHeartbeatInterval(int interval)
        {
            _logger.LogDebug($"Heartbeat interval set to: {interval}");
            _interval = interval;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                _logger.LogDebug($"Heartbeat executing");
                await _wsService.SendAsync(
                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new gateway_heartbeat { d = null })),
                    System.Net.WebSockets.WebSocketMessageType.Text,
                    cancellation
                );
                await Task.Delay(_interval, cancellation);
            }
        }
    }
}