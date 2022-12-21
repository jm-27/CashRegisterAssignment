// See https://aka.ms/new-console-template for more information
using CashRegisterAssignment;
using CashRegisterAssignment.ChangeCalculator;
using CashRegisterAssignment.ChangeCalculator.Interfaces;
using CashRegisterAssignment.ChangeCalculator.Models;
using CashRegisterAssignment.ChangeCalculator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = new ConfigurationBuilder();
builder.AddJsonFile("appsettings.json");
var configRoot = builder.Build();

//Prepares the bundle of services to be used with DI
var services = new ServiceCollection();
services.AddTransient<IChangeCalculatorService, ChangeCalculatorService>();
services.AddTransient<ICurrencyStore, CurrencyStore>();
services.AddTransient<IChangeCalculator, ChangeCalculator>();
services.AddTransient<ICashWrapper>(p => new CashWrapper(ICashWrapper.Status.Initial,"",null));
services.AddTransient<ICashAmount>(p => new CashAmount(0,0.00m));
services.AddTransient<IChangeCalculatorLogService<ChangeCalculator>, ChangeCalculatorLogService<ChangeCalculator>>();
services.AddSingleton<CashRegisterDemo>();

// Add logging
services.AddLogging(configure =>
{
    configure.AddConfiguration(configRoot.GetSection("Logging"));
    configure.AddSerilog(new LoggerConfiguration().WriteTo.File("logs.txt")
        .CreateLogger());
    configure.AddConsole();    
}).AddTransient<ChangeCalculator>();

var _serviceProvider = services.BuildServiceProvider(true);

//Starts the demonstration application.
IServiceScope scope = _serviceProvider.CreateScope();
scope.ServiceProvider.GetRequiredService<CashRegisterDemo>().Run();

