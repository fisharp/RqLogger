using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

using RqLogger.Logger.Service;

namespace RqLogger.Logger.Extensions
{
	public static class RqLoggerExtensions
	{
		public static ILoggingBuilder AddRqLogger(
			this ILoggingBuilder builder)
		{
			builder.AddConfiguration();

			builder.Services.TryAddEnumerable(
				ServiceDescriptor.Singleton<ILoggerProvider, RqLoggerProvider>());

			LoggerProviderOptions.RegisterProviderOptions
				<RqLoggerConfiguration, RqLoggerProvider>(builder.Services);

			return builder;
		}

		public static ILoggingBuilder AddRqLogger(
			this ILoggingBuilder builder,
			Action<RqLoggerConfiguration> configure)
		{
			builder.AddRqLogger();
			builder.Services.Configure(configure);

			return builder;
		}
	}
}