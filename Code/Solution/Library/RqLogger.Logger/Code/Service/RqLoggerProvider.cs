using System;
using System.Collections.Concurrent;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RqLogger.Logger.Service
{
	public class RqLoggerProvider : ILoggerProvider
	{
		private readonly IDisposable _OnChangeToken;

		private readonly ConcurrentDictionary<string, RqLogger> _Loggers = new ConcurrentDictionary<string, RqLogger>();

		private IHttpContextAccessor _ContextAccessor;

		private RqLoggerConfiguration _CurrentConfig;

		public RqLoggerProvider(
			IOptionsMonitor<RqLoggerConfiguration> config,
			IHttpContextAccessor contextAccessor)
		{
			_CurrentConfig = config.CurrentValue;
			_OnChangeToken = config.OnChange(updatedConfig => _CurrentConfig = updatedConfig);
			_ContextAccessor = contextAccessor;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return _Loggers.GetOrAdd(categoryName, name => new RqLogger(name, _CurrentConfig, _ContextAccessor));
		}

		public void Dispose()
		{
			_Loggers.Clear();
			_OnChangeToken.Dispose();
		}
	}
}