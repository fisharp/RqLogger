using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RqLogger.Demo.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _Logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_Logger = logger;
			logger.LogInformation("Test Controller initialized");
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			_Logger.LogInformation(15, "Starting Get");
			var rng = new Random();
			WeatherForecast[] response = Enumerable.Range(1, 5).Select(index => new WeatherForecast
				{
					Date = DateTime.Now.AddDays(index),
					TemperatureC = rng.Next(-20, 55),
					Summary = Summaries[rng.Next(Summaries.Length)]
				})
				.ToArray();
			_Logger.LogInformation(15, "Completed with {Len} results", response.Length);
			return response;
		}
	}
}