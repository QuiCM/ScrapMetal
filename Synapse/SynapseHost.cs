using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Synapse
{
    public class SynapseHost
    {
        public static async Task Run()
        {
            await Task.Run(() =>
            {
                new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls("https://*:8443")
                    .UseStartup<Startup>()
                    .Build()
                .Run();
            });
        }
    }

    public class Startup
    {
        [SuppressMessage("csharp", "CA1822")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        [SuppressMessage("csharp", "CA1822")]
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}