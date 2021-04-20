using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Serilog.Events;

using System;

using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace BookingService.Controllers
{
    [Route("booking/api/v1/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly ILogger _logger;

        public StatusController(ILogger<StatusController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: lifetime of the application in seconds
        /// </summary>
        [HttpGet("Uptime")]
        public string Uptime()
        {
            _logger.LogDebug($"{nameof(Uptime)} invoked.");

            var uptime = Service.Uptime;
            return $"{uptime.Hours}:{uptime.Minutes}:{uptime.Seconds}";
        }

        /// <summary>
        /// gets log level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [HttpGet("GetLogLevel")]
        public int GetLogLevel()
        {
            var level = (int)Service.LoggingLevelSwitch.MinimumLevel;
            _logger.LogInformation("Getting current log level: {Level}", level);

            return level;
        }

        [HttpGet("SetLogLevel")]
        public string SetLogLevel(int level)
        {
            _logger.LogInformation("switching log level to {Level}", level);

            Service.LoggingLevelSwitch.MinimumLevel = (LogEventLevel)level;

            return $"switched to: {level}";
        }
    }
}
