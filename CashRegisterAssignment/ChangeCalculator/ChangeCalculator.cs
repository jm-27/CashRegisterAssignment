using CashRegisterAssignment.ChangeCalculator.CustomExceptions;
using CashRegisterAssignment.ChangeCalculator.Interfaces;
using CashRegisterAssignment.ChangeCalculator.Models;


namespace CashRegisterAssignment.ChangeCalculator
{
    public class ChangeCalculator : IChangeCalculator
    {
        IChangeCalculatorService _changeCalculatorService;
        ICashWrapper _cashWrapper;
        IChangeCalculatorLogService<ChangeCalculator> _logService;
        
        public ChangeCalculator(IChangeCalculatorService changeCalculatorService,
            IChangeCalculatorLogService<ChangeCalculator> logService, ICashWrapper cashWrapper)
        {
            _changeCalculatorService = changeCalculatorService;
            _cashWrapper =  cashWrapper; ;
            _logService = logService;
        }

        /// <summary>
        /// Asks the service to Set the Currency Store Collection.
        /// </summary>
        /// <param name="currenciesCollection"></param>
        public void SetCurrencyStoreCollection(List<CurrencyItem> currenciesCollection)
        {
            _changeCalculatorService.SetCurrencyStoreCollection(currenciesCollection);
        }

        /// <summary>
        /// Asks the service to Set an Active Currency.
        /// </summary>
        /// <param name="activeCurrency"></param>
        public void SetActiveCurrency(string activeCurrency)
        {
            _changeCalculatorService.SetActiveCurrency(activeCurrency);
        }

        /// <summary>
        /// Asks the service to get the name of the active currency.
        /// </summary>
        /// <returns></returns>
        public string GetActiveCurrency()
        {
            //Returns the name of the currency
            return _changeCalculatorService.GetActiveCurrency().Name;
        }

        /// <summary>
        /// Gets the list of Accepted Coins and Bills that can be
        /// processed by the active currency. Returns a string of 
        /// denominations that can be printed on screen.
        /// </summary>
        /// <returns></returns>
        public string GetAcceptedCoinsAndBills()
        {
            string acceptedCoinsAndBills = String.Empty;
            foreach (var item in _changeCalculatorService.GetActiveCurrency().Denominations)
            {
                acceptedCoinsAndBills += item.ToString() + "\r\n";
            }
            return acceptedCoinsAndBills;
        }

        /// <summary>
        /// Recieves the list of prices of the products being aquired 
        /// and the amount of cash that the customer has to pay the total.
        /// If the amount of cash is valid (has correct denominations) then
        /// the method will return one of the following:
        /// 1.- A list of Cash Amount to return to the customer.
        /// 2.- If the cash is okay and there's no amount to be returned return a 
        ///    Cash Amount list with zero quantity and zero denominations.
        /// 3.- If the cash that the customer has given is not enough raise a Insufficient Cash Exception.   
        /// </summary>
        /// <param name="ItemsPrices"></param>
        /// <param name="cashAmounts"></param>
        /// <returns></returns>
        /// <exception cref="InsufficientCashException"></exception>
        /// <exception cref="IncorrectMoneyInputException"></exception>
        public ICashWrapper SubmitCash(List<decimal> ItemsPrices, List<ICashAmount> cashAmounts)
        {
            
            try
            {
                _changeCalculatorService.CashCollection = cashAmounts;
                if (_changeCalculatorService.isValid)
                {
                    decimal cashToReturn = _changeCalculatorService.CashCollection.Sum(x => (x.Quantity * x.Denomination)) - ItemsPrices.Sum();
                    if (cashToReturn > 0)
                    {
                        return _cashWrapper.SetCashWrapperData(ICashWrapper.Status.Valid, GetActiveCurrency(), ReturnCash(cashToReturn));
                    }
                    else if (cashToReturn == 0)
                    {
                        return _cashWrapper.SetCashWrapperData(ICashWrapper.Status.Valid, GetActiveCurrency(), new List<ICashAmount>() { new CashAmount() { Quantity = 0, Denomination = 0.00m } });
                    }
                    else
                    {                        
                        throw new InsufficientCashException($"Insufficient cash. Deficit of ${Math.Abs(cashToReturn)} remains.");                        
                    }
                }
                else
                {
                    throw new IncorrectMoneyInputException("Invalid cash input. Please review the currency denominations.");
                }
            }
            catch (InsufficientCashException ex)
            {
                _logService.AddLog(IChangeCalculatorLogService<ChangeCalculator>.Level.Warning, "Insufficient cash.",ex);
                return _cashWrapper.SetCashWrapperData(ICashWrapper.Status.InsufficientCash, GetActiveCurrency(), null);
            }
            catch (IncorrectMoneyInputException ex)
            {
                _logService.AddLog(IChangeCalculatorLogService<ChangeCalculator>.Level.Error, "Invalid cash input.", ex);
                return _cashWrapper.SetCashWrapperData(ICashWrapper.Status.InvalidInput, GetActiveCurrency(), null);
            }
            catch(Exception ex)
            {
                _logService.AddLog(IChangeCalculatorLogService<ChangeCalculator>.Level.Error, "Exception raised. See exception details.", ex);
                return _cashWrapper.SetCashWrapperData(ICashWrapper.Status.Invalid, GetActiveCurrency(), null);
            }


        }


        /// <summary>
        /// Returns the amount of cash to the customer.
        /// It return the smallest number of bills and coins equal to the change due.
        /// The param amount is the total of cash to be returned.
        /// The return type is a List of Cash Denominations that the customer will recieve.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// <exception cref="EmptyCurrencyException"></exception>
        public List<ICashAmount> ReturnCash(decimal amount)
        {
            decimal amountToCash = amount;
            decimal denomination = 0;

            List<ICashAmount> cashToReturn = new List<ICashAmount>();
            List<decimal> denominationList = _changeCalculatorService.GetActiveCurrency().Denominations;
            while (amountToCash > 0)
            {
                try
                {
                    denomination = denominationList.Where(x => amountToCash - x >= 0).FirstOrDefault();
                    cashToReturn.Add(new CashAmount
                    {
                        Quantity = (int)Math.Floor(amountToCash / denomination),
                        Denomination = denomination
                    });
                    amountToCash -= denomination * (int)Math.Floor(amountToCash / denomination);
                }
                catch (Exception)
                {
                    throw new CurrencyDataNotPresentException("Check currency data store.");
                }

            }
            return cashToReturn;
        }
    }
}
