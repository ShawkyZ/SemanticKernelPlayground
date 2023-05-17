using Microsoft.Extensions.Logging;

namespace SemanticKernelPlayground.RepoUtils
{
    internal static class ConsoleLogger
    {
        internal static ILogger Log => LogFactory.CreateLogger<object>();

        private static ILoggerFactory LogFactory => s_loggerFactory.Value;

        private static readonly Lazy<ILoggerFactory> s_loggerFactory = new(LogBuilder);

        private static ILoggerFactory LogBuilder()
        {
            return LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Warning);
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.AddFilter("System", LogLevel.Warning);

                builder.AddConsole();
            });
        }
    }
}
