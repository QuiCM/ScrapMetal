using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CandyBar.http.workers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CandyBar
{
    public class DiscordApiService : BackgroundService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<DiscordApiService> _logger;
        private readonly DiscordOptions _options;
        private CancellationToken _cancellation;

        public DiscordApiService(IHttpClientFactory clientFactory, ILogger<DiscordApiService> logger, IOptions<DiscordOptions> options)
        {
            _logger = logger;
            _options = options.Value ?? throw new Exception("Discord options not set"); //TODO: add better exceptions
            _clientFactory = clientFactory;
        }

        public async Task<Uri> GetGatewayUriAsync()
        {
            var client = _clientFactory.CreateClient(nameof(DiscordApiService));
            _logger.LogInformation("Requesting gateway with auth: " + string.Join(",", client.DefaultRequestHeaders.GetValues("Authorization")));
            GatewayWorker worker = new(client);
            return await worker.GetGatewayUri(_cancellation);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cancellation = stoppingToken;
            return Task.CompletedTask;
        }
    }
}