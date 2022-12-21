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
        public ICashAmount _cashAmount;
        public ChangeCalculatorTests(TestFixtures testFixtures)
        {
            _serviceProvider = testFixtures.serviceProvider;
            currenciesCollection = testFixtures.currenciesCollection;
            _cashAmount = testFixtures.CashAmount;

        }

        [Fact]
        public void Submit_With_Extra_Cash_Return_Cash_Amount()
        {
            IChangeCalculator _changeCalculator = _serviceProvider.GetService<IChangeCalculator>();
            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency("USD");

            List<decimal> priceList = new List<decimal>() { 15.00m, 0.26m };

            List<ICashAmount> cashAmounts = new List<ICashAmount>();
            cashAmounts.Add(_cashAmount.SetCashAmountData(2,  10.00m ));
            cashAmounts.Add(_cashAmount.SetCashAmountData(1, 0.50m ));

            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(_cashAmount.SetCashAmountData(1, 5.00m ));
            expectedCashAmounts.Add(_cashAmount.SetCashAmountData(2,  0.10m));
            expectedCashAmounts.Add(_cashAmount.SetCashAmountData(4, 0.01m ));

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
            cashAmounts.Add(_cashAmount.SetCashAmountData(1,  10.00m ));
            cashAmounts.Add(_cashAmount.SetCashAmountData(1,  5.00m ));
            cashAmounts.Add(_cashAmount.SetCashAmountData(1,  0.25m ));

            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(_cashAmount.SetCashAmountData(0,  0.00m));

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
            cashAmounts.Add(_cashAmount.SetCashAmountData(1, 10.00m));

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
            cashAmounts.Add(_cashAmount.SetCashAmountData(1, 6.00m));

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
            cashAmounts.Add(_cashAmount.SetCashAmountData( 1,  50.00m ));


            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);


            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(_cashAmount.SetCashAmountData(1,  20.00m ));
            expectedCashAmounts.Add(_cashAmount.SetCashAmountData(2, 2.00m ));
            expectedCashAmounts.Add(_cashAmount.SetCashAmountData( 1,  0.50m ));

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
            cashAmounts.Add(_cashAmount.SetCashAmountData(1,  50.00m ));
            cashAmounts.Add(_cashAmount.SetCashAmountData(2, 20.00m));
            cashAmounts.Add(_cashAmount.SetCashAmountData(1,  2.00m ));
            cashAmounts.Add(_cashAmount.SetCashAmountData(1,  0.20m ));
            cashAmounts.Add(_cashAmount.SetCashAmountData(1,  0.05m ));

            ICashWrapper result =
                _changeCalculator.SubmitCash(priceList, cashAmounts);

            List<ICashAmount> expectedCashAmounts = new List<ICashAmount>();
            expectedCashAmounts.Add(_cashAmount.SetCashAmountData(0, 0.00m ));

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
            cashAmounts.Add(_cashAmount.SetCashAmountData(12, 10.00m ));

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
            cashAmounts.Add(_cashAmount.SetCashAmountData( 1, 70.00m ));

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