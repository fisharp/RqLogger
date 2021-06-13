using System.Collections.Generic;

using Microsoft.Extensions.Logging;

namespace RqLogger.Logger
{
	public class RqLoggerConfiguration
	{
		public int EventId { get; set; }

		public Dictionary<LogLevel, string> LogLevels { get; set; } = new Dictionary<LogLevel, string>
		{
			[LogLevel.Information] = "INFO"
		};
	}
}