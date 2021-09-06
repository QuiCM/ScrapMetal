using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScrapMetal
{
    public class ScrapMetalService : IHostedService
    {
        private readonly ILogger<ScrapMetalService> _logger;
        private readonly ScrapMetalOptions _options;
        private readonly GatewayService _gatewayService;

        public ScrapMetalService(ILogger<ScrapMetalService> logger, IOptions<ScrapMetalOptions> options, GatewayService gatewayService)
        {
            _logger = logger;
            _options = options.Value ?? throw new System.Exception("Config borked");
            _gatewayService = gatewayService;
        }

        public async Task StartAsync(CancellationToken cancellation)
        {
            _logger.LogInformation("StartAsync called");
            await _gatewayService.StartAsync(cancellation);
        }

        public async Task StopAsync(CancellationToken cancellation)
        {
            _logger.LogInformation("StopAsync called");
            await _gatewayService.StopAsync(cancellation);
        }
    }
}