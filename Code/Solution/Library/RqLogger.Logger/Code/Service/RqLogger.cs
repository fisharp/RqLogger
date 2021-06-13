using System;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using RqLogger.Logger.Middleware;

namespace RqLogger.Logger.Service
{
	public class RqLogger : ILogger
	{
		private readonly string _Name;
		private readonly RqLoggerConfiguration _Config;
		private readonly IHttpContextAccessor? _ContextAccessor;

		public RqLogger(
			string name,
			RqLoggerConfiguration config,
			IHttpContextAccessor accessor)
		{
			(_Name, _Config, _ContextAccessor) = (name, config, accessor);
		}

		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception exception,
			Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;

			if (LogManagerFound())
			{
				var manager = (RqLoggerManager)_ContextAccessor!.HttpContext.Items["LogManager"];
				manager.AppendLog(eventId.Id, logLevel, formatter(state, exception), exception);
			}
			else
				Console.WriteLine($"[{eventId.Id,2}: {_Config.LogLevels[logLevel],-5}] {_Name} - {formatter(state, exception)}");
		}

		public bool LogManagerFound() =>
			_ContextAccessor?.HttpContext?.Request != null &&
			_ContextAccessor.HttpContext.Items.ContainsKey("LogManager");

		public bool IsEnabled(LogLevel logLevel) =>
			_Config.LogLevels.ContainsKey(logLevel);

		public IDisposable BeginScope<TState>(TState state) => default!;
	}
}