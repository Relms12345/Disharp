using Serilog;

namespace Disharp.Utils
{
	public static class Logger
	{
		public static Serilog.Core.Logger Log()
		{
			var log = new LoggerConfiguration()
				.WriteTo.Console()
				.MinimumLevel.Verbose()
				.CreateLogger();

			return log;
		}
	}
}