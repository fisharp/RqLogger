namespace RqLogger.Middleware.ViewModels
{
	public class ErrorDetails
	{
		public int StatusCode { get; set; }

		public string? Title { get; set; }

		public string? Error { get; set; }

		public string? Source { get; set; }

		public string? Request { get; set; }

		public string? RootCause { get; set; }
	}
}