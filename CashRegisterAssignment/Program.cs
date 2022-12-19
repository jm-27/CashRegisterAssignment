// See https://aka.ms/new-console-template for more information
using CashRegisterAssignment;
using CashRegisterAssignment.ChangeCalculator;
using CashRegisterAssignment.ChangeCalculator.Interfaces;
using CashRegisterAssignment.ChangeCalculator.Models;
using CashRegisterAssignment.ChangeCalculator.Services;
using Microsoft.Extensions.DependencyInjection;


//Prepares the bundle of services to be used with DI
var services = new ServiceCollection();
services.AddTransient<IChangeCalculatorService, ChangeCalculatorService>();
services.AddTransient<ICurrencyStore, CurrencyStore>();
services.AddTransient<IChangeCalculator, ChangeCalculator>();
services.AddSingleton<CashRegisterDemo>();
var _serviceProvider = services.BuildServiceProvider(true);

//Starts the demonstration application.
IServiceScope scope = _serviceProvider.CreateScope();
scope.ServiceProvider.GetRequiredService<CashRegisterDemo>().Run();

