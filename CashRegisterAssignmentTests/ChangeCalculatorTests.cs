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
        List<CurrencyItem> currenciesCollection;
        public ChangeCalculatorTests(TestFixtures testFixtures)
        {
            _serviceProvider = testFixtures.serviceProvider;
            currenciesCollection = testFixtures.currenciesCollection;

        }

        [Fact]
        public void Submit_With_Extra_Cash_Return_Cash_Amount()
        {
            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("USD");

            List<decimal> priceList = new List<decimal>() { 15.00m, 0.26m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 2, Denomination = 10.00m });
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 0.50m });

            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 5.00m });
            expectedCashAmounts.Add(new CashAmount() { Quantity = 2, Denomination = 0.10m });
            expectedCashAmounts.Add(new CashAmount() { Quantity = 4, Denomination = 0.01m });

            ICashWrapper cashWrapper = new CashWrapper(ICashWrapper.Status.Valid, _changeCalculator.GetActiveCurrency(), expectedCashAmounts);

            Assert.Equivalent(cashWrapper, result);
        }

        [Fact]
        public void Submit_Exact_Cash_Amount()
        {
            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("USD");

            List<decimal> priceList = new List<decimal>() { 5.00m, 5.00m, 0.25m, 5.00m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 10.00m });
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 5.00m });
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 0.25m });

            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(new CashAmount() { Quantity = 0, Denomination = 0.00m });

            ICashWrapper cashWrapper = new CashWrapper(ICashWrapper.Status.Valid, _changeCalculator.GetActiveCurrency(), expectedCashAmounts);

            Assert.Equivalent(cashWrapper, result);
        }

        [Fact]
        public void Submit_Insufficient_Cash_Amount()
        {
            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("USD");

            List<decimal> priceList = new List<decimal>() { 5.00m, 5.00m, 0.25m, 5.00m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 10.00m });

            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            Assert.Equal(ICashWrapper.Status.InsufficientCash, ((CashWrapper)result).Status);
        }

        [Fact]
        public void Submit_Incorrect_Money_Input()
        {
            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("USD");

            List<decimal> priceList = new List<decimal>() { 5.00m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 6.00m });

            ICashWrapper result =
            _changeCalculator.SubmitCash(priceList, cashAmounts);


            Assert.Equal(ICashWrapper.Status.InvalidInput, ((CashWrapper)result).Status);
        }

        [Fact]
        public void Submit_MXN_With_Extra_Cash_Return_Cash_Amount()
        {
            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("MXN");

            List<decimal> priceList = new List<decimal>() { 20.00m, 5.00m, 0.50m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 50.00m });


            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);


            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 20.00m });
            expectedCashAmounts.Add(new CashAmount() { Quantity = 2, Denomination = 2.00m });
            expectedCashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 0.50m });

            ICashWrapper cashWrapper = new CashWrapper(ICashWrapper.Status.Valid, _changeCalculator.GetActiveCurrency(), expectedCashAmounts);

            Assert.Equivalent(cashWrapper, result);
        }

        [Fact]
        public void Submit_MXN_Exact_Cash_Amount()
        {
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

            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(new CashAmount() { Quantity = 0, Denomination = 0.00m });

            ICashWrapper cashWrapper = new CashWrapper(ICashWrapper.Status.Valid, _changeCalculator.GetActiveCurrency(), expectedCashAmounts);

            Assert.Equivalent(cashWrapper, result);
        }

        [Fact]
        public void Submit_MXN_Insufficient_Cash_Amount()
        {
            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("MXN");

            List<decimal> priceList = new List<decimal>() { 105.00m, 15.75m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 12, Denomination = 10.00m });

            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            Assert.Equal(ICashWrapper.Status.InsufficientCash, ((CashWrapper)result).Status);
        }

        [Fact]
        public void Submit_MXN_Incorrect_Money_Input()
        {
            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("MXN");

            List<decimal> priceList = new List<decimal>() { 50.00m, 20.00m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(new CashAmount() { Quantity = 1, Denomination = 70.00m });

            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            Assert.Equal(ICashWrapper.Status.InvalidInput, ((CashWrapper)result).Status);
        }

        [Fact]
        public void Set_Unexistent_Currency_In_Storage_Throw_Exception()
        {
            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
  
            Assert.Throws<CurrencyDataNotPresentException>(() => _changeCalculator.SetActiveCurrency("XYZ"));
        }
    }
}