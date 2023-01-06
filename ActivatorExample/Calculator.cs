using Microsoft.Extensions.Logging;

namespace ActivatorExample
{
    internal class CalculatorExample
    {
        private readonly ILogger<CalculatorExample> _logger;
        private readonly LoggerDictionary? _loggerRepository;

        public CalculatorExample(ILogger<CalculatorExample> logger)
        {
            _logger = logger;
        }

        public CalculatorExample(ILogger<CalculatorExample> logger, LoggerDictionary loggerRepository)
        {
            _logger = logger;
            _loggerRepository = loggerRepository;
        }

        public int Add(int a, int b)
        {
            var result = a + b;
            var message = $"Adding {a}, {b} which is {result}.";
            _logger.LogInformation(message);
            _loggerRepository?.AddToLogRepo(message);
            return result;
        }
    }
}
