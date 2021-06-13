using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using RqLogger.Logger.Extensions;

namespace RqLogger.Demo
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			using IHost host = CreateHostBuilder(args).Build();

			ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();

			logger.LogDebug(1, "Starting logs from Main");
			logger.LogInformation(3, "First Info log entry");
			logger.LogWarning(5, "This is a Warning!");
			logger.LogError(7, "Oops, there was an error");

			await host.RunAsync();
		}

		private static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureLogging(builder =>
					builder.ClearProviders()
						.AddRqLogger(config =>
						{
							config.LogLevels.Add(
								LogLevel.Warning, "WARN");
							config.LogLevels.Add(
								LogLevel.Error, "ERR");
						}))
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
		}
	}
}