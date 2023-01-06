using ActivatorExample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<ICreateServiceFactory, CreateServiceFactory>();
        services.AddSingleton<CalculatorExample>();
        services.AddLogging();
    }).Build();



var factory = host.Services.GetRequiredService<ICreateServiceFactory>();
var diCalculatorExample = host.Services.GetRequiredService<CalculatorExample>();



diCalculatorExample.Add(8, 8);

var loggerRepo = new LoggerDictionary(host.Services.GetRequiredService<ILogger<LoggerDictionary>>());

var factoryCalculatorExample = factory.CreateService<CalculatorExample>(loggerRepo);

factoryCalculatorExample.Add(5, 8);



var loggerRepoForManual = new LoggerDictionary(host.Services.GetRequiredService<ILogger<LoggerDictionary>>());
var calculatorLogger = host.Services.GetRequiredService<ILogger<CalculatorExample>>();

var manualCalculatorExample = new CalculatorExample(calculatorLogger, loggerRepoForManual);

manualCalculatorExample.Add(9,9);