using System;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Logging;

namespace RqLogger.Logger
{
	public class LogEntry
	{
		[JsonIgnore]
		public Guid RequestId { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public LogLevel Level { get; set; }

		public int EventId { get; set; }

		public string Message { get; set; }

		/// <summary>
		/// Elapsed milliseconds since initialization of the request Queue
		/// </summary>
		public long ElapsedTime { get; set; }

		public Exception Exception { get; set; }
	}
}