using CashRegisterAssignment.ChangeCalculator.CustomExceptions;
using CashRegisterAssignment.ChangeCalculator.Interfaces;
using CashRegisterAssignment.ChangeCalculator.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static CashRegisterAssignment.ChangeCalculator.ChangeCalculator;

namespace CashRegisterAssignmentTests
{
    public class ChangeCalculatorTests : IClassFixture<TestFixtures>
    {
        private ServiceProvider _serviceProvider;
        public ChangeCalculatorTests(TestFixtures testFixtures)
        {
            _serviceProvider = testFixtures.serviceProvider;
        }

        [Fact]
        public void Submit_With_Extra_Cash_Return_Cash_Amount()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            List<KeyValuePair<string, List<decimal[]>>> currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<Dictionary<string, List<decimal[]>>>().ToList();

            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("USD");


            List<decimal> priceList = new List<decimal>() { 15.00m, 0.26m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 2, Denomination = 10.00m });
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 0.50m });

            List<ICashAmount> result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 5.00m });
            expectedCashAmounts.Add(new CashAmount() { Quantity = 2, Denomination = 0.10m });
            expectedCashAmounts.Add(new CashAmount() { Quantity = 4, Denomination = 0.01m });

            Assert.Equivalent(expectedCashAmounts, result);
        }

        [Fact]
        public void Submit_Exact_Cash_Amount()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            List<KeyValuePair<string, List<decimal[]>>> currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<Dictionary<string, List<decimal[]>>>().ToList();

            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("USD");

            List<decimal> priceList = new List<decimal>() { 5.00m, 5.00m, 0.25m, 5.00m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 10.00m });
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 5.00m });
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 0.25m });

            List<ICashAmount> result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(new CashAmount() { Quantity = 0, Denomination = 0.00m });

            Assert.Equivalent(expectedCashAmounts, result);
        }

        [Fact]
        public void Submit_Insufficient_Cash_Amount()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            List<KeyValuePair<string, List<decimal[]>>> currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<Dictionary<string, List<decimal[]>>>().ToList();

            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("USD");

            List<decimal> priceList = new List<decimal>() { 5.00m, 5.00m, 0.25m, 5.00m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 10.00m });

            Assert.Throws<InsufficientCashException>(() => _changeCalculator.SubmitCash(priceList, cashAmounts));
        }

        [Fact]
        public void Submit_Incorrect_Money_Input()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            List<KeyValuePair<string, List<decimal[]>>> currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<Dictionary<string, List<decimal[]>>>().ToList();

            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("USD");

            List<decimal> priceList = new List<decimal>() { 5.00m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 6.00m });

            Assert.Throws<IncorrectMoneyInputException>(() => _changeCalculator.SubmitCash(priceList, cashAmounts));
        }

        [Fact]
        public void Submit_MXN_With_Extra_Cash_Return_Cash_Amount()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            List<KeyValuePair<string, List<decimal[]>>> currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<Dictionary<string, List<decimal[]>>>().ToList();

            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("MXN");

            List<decimal> priceList = new List<decimal>() { 20.00m, 5.00m, 0.50m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 50.00m });


            List<ICashAmount> result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);


            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 20.00m });
            expectedCashAmounts.Add(new CashAmount() { Quantity = 2, Denomination = 2.00m });
            expectedCashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 0.50m });

            Assert.Equivalent(expectedCashAmounts, result);
        }

        [Fact]
        public void Submit_MXN_Exact_Cash_Amount()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            List<KeyValuePair<string, List<decimal[]>>> currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<Dictionary<string, List<decimal[]>>>().ToList();

            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("MXN");

            List<decimal> priceList = new List<decimal>() { 50.00m, 25.00m, 17.25m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 50.00m });
            cashAmounts.Add(new CashAmount() { Quantity = 2, Denomination = 20.00m });
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 2.00m });
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 0.20m });
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 0.05m });

            List<ICashAmount> result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(new CashAmount() { Quantity = 0, Denomination = 0.00m });

            Assert.Equivalent(expectedCashAmounts, result);
        }

        [Fact]
        public void Submit_MXN_Insufficient_Cash_Amount()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            List<KeyValuePair<string, List<decimal[]>>> currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<Dictionary<string, List<decimal[]>>>().ToList();

            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);     
            _changeCalculator.SetActiveCurrency("MXN");

            List<decimal> priceList = new List<decimal>() { 105.00m, 15.75m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 12, Denomination = 10.00m });

            Assert.Throws<InsufficientCashException>(() => _changeCalculator.SubmitCash(priceList, cashAmounts));
        }

        [Fact]
        public void Submit_MXN_Incorrect_Money_Input()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            List<KeyValuePair<string, List<decimal[]>>> currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<Dictionary<string, List<decimal[]>>>().ToList();

            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("MXN");

            List<decimal> priceList = new List<decimal>() { 50.00m, 20.00m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 70.00m });

            Assert.Throws<IncorrectMoneyInputException>(() => _changeCalculator.SubmitCash(priceList, cashAmounts));
        }
    }
}