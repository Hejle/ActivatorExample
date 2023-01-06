using ActivatorExample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

//Creates Host to use with Microsoft DependencyInjection
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<ICreateServiceFactory, CreateServiceFactory>();
        //Adds CalculatorExample to DI
        services.AddSingleton<CalculatorExample>();
        services.AddLogging();
    }).Build();

var factory = host.Services.GetRequiredService<ICreateServiceFactory>();
//Creates CalculatorExample from DependencyInjection.
//Note that this will use the Constructor that only accepts ILogger, ignoring the LoggerDictionary since LoggerDictionary is not in DI. 
var diCalculatorExample = host.Services.GetRequiredService<CalculatorExample>();

diCalculatorExample.Add(8, 8);

//Creates a LoggerDictionary used as input to create a CalculatorExample using the CreateServiceFactory
var loggerRepo = new LoggerDictionary(host.Services.GetRequiredService<ILogger<LoggerDictionary>>());

//Creating a CalculatorExample by having the CreateServiceFactory matching types, using both services from the parameters as well as services from DI.
var factoryCalculatorExample = factory.CreateService<CalculatorExample>(loggerRepo);

factoryCalculatorExample.Add(5, 8);

//Manually creates a CalculatorExample
var loggerRepoForManual = new LoggerDictionary(host.Services.GetRequiredService<ILogger<LoggerDictionary>>());
var calculatorLogger = host.Services.GetRequiredService<ILogger<CalculatorExample>>();

var manualCalculatorExample = new CalculatorExample(calculatorLogger, loggerRepoForManual);

manualCalculatorExample.Add(9,9);