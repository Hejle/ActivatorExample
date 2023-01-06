using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ActivatorExample
{
    internal class LoggerDictionary
    {
        private readonly Dictionary<DateTime, string> _logRepo = new Dictionary<DateTime, string>();
        private readonly ILogger<LoggerDictionary> _logger;

        public LoggerDictionary(ILogger<LoggerDictionary> logger)
        {
            _logger = logger;
        }

        public void AddToLogRepo(string message)
        {
            _logger.LogInformation($"Logging message '{message}' to Repo");
            _logRepo.Add(DateTime.Now, message);
        }
    }
}
