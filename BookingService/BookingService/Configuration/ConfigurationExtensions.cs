

using System;
using Microsoft.Extensions.Configuration;

namespace BookingService.Configuration
{
    /// <summary>
    /// Extension methods for the IConfiguration interface.
    /// </summary>
    public static class ConfigurationExtensions
    {
        public static string GetServiceName(this IConfiguration configuration)
        {
            return configuration["ServiceName"] ?? Environment.CurrentDirectory;
        }

        public static string GetLogFilePath(this IConfiguration configuration)
        {
            return configuration["LogFilePath"] ?? Environment.CurrentDirectory;
        }
    }
}
