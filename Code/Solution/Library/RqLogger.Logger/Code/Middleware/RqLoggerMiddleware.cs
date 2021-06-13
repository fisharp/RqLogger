using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using RqLogger.Logger.Interfaces;

namespace RqLogger.Logger.Middleware
{
	public class RqLoggerMiddleware
	{
		private readonly ILogger<RqLoggerMiddleware> _Logger;
		private readonly RequestDelegate _Next;
		private readonly IRqLogFormatter _Formatter;

		public RqLoggerMiddleware(
			RequestDelegate next,
			ILogger<RqLoggerMiddleware> logger,
			IRqLogFormatter formatter)
		{
			(_Next, _Logger, _Formatter) = (next, logger, formatter);
		}

		public async Task Invoke(HttpContext context)
		{
			using var manager = new RqLoggerManager();
			context.Items.Add("LogManager", manager);
			_Logger.LogInformation(1, "Init Middleware");

			await _Next(context);

			_Logger.LogInformation(1, "End of Middleware");
			Console.WriteLine(_Formatter.Format(manager));
		}
	}
}