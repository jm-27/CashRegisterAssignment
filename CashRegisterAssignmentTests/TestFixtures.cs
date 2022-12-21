
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Security.Cryptography.X509Certificates;

namespace CashRegisterAssignmentTests
{
    public class TestFixtures
    {
        public ServiceProvider serviceProvider { get; set; }
        public List<CurrencyItem> currenciesCollection { get; set; }
        public TestFixtures()
        {

            IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<List<CurrencyItem>>();


            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configRoot = builder.Build();

            var services = new ServiceCollection();
            services.AddTransient<IChangeCalculatorService, ChangeCalculatorService>();
            services.AddTransient<ICurrencyStore, CurrencyStore>();
            services.AddTransient<IChangeCalculator, ChangeCalculator>();
            services.AddTransient<ICashWrapper>(p => new CashWrapper(ICashWrapper.Status.Initial, "", null));
            services.AddTransient<IChangeCalculatorLogService<ChangeCalculator>, ChangeCalculatorLogService<ChangeCalculator>>();
            services.AddLogging(configure =>
            {
                configure.AddConfiguration(configRoot.GetSection("Logging"));
                configure.AddSerilog(new LoggerConfiguration().WriteTo.File("logs.txt")
                    .CreateLogger());
                configure.AddConsole();
            }).AddTransient<ChangeCalculator>();
            serviceProvider = services.BuildServiceProvider(true);
        }
    }

}
