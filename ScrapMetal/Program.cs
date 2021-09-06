using System;
using System.Threading;
using System.Threading.Tasks;
using CandyBar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeakEasy;

namespace ScrapMetal
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new();
            using IHost host = CreateHost(args ?? Array.Empty<string>());

            await host.StartAsync(cancellationTokenSource.Token);
            await host.WaitForShutdownAsync(cancellationTokenSource.Token);
        }

        static IHost CreateHost(string[] args) => Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(conf =>
                {
                    conf.AddJsonFile("launchconfig.json", optional: true, reloadOnChange: true);
                    conf.AddEnvironmentVariables(prefix: "ScrapMetal_");
                    conf.AddEnvironmentVariables(prefix: "SpeakEasy_");
                    if (args.Length > 0)
                    {
                        conf.AddCommandLine(args);
                    }
                }).ConfigureLogging(conf =>
                    conf.ClearProviders()
                        .AddConsole()
                        .AddDebug()
                        .SetMinimumLevel(LogLevel.Debug)
                ).ConfigureServices((ctx, services) =>
                {
                    services.Configure<DiscordOptions>(ctx.Configuration.GetSection("Discord"))
                            .Configure<ScrapMetalOptions>(ctx.Configuration.GetSection("ScrapMetal"))
                            .Configure<WebSocketOptions>(ctx.Configuration.GetSection("SpeakEasy"));

                    services.AddHttpClient<DiscordApiService>()
                            .ConfigureHttpClient(client =>
                            {
                                client.BaseAddress = new("https://discord.com/api/");
                                client.DefaultRequestHeaders.Add("Authorization", $"Bot {ctx.Configuration.GetSection("Discord")["ApiToken"]}");
                            });

                    services.AddSingleton<GatewayService>()
                            .AddSingleton<DiscordApiService>()
                            .AddSingleton<SpeakEasySocketService>()
                            .AddSingleton<HeartbeatService>()
                            .AddHostedService<ScrapMetalService>();
                })
                .Build();
    }
}
