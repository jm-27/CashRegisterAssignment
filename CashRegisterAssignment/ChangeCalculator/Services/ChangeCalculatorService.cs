using CashRegisterAssignment.ChangeCalculator.CustomExceptions;
using CashRegisterAssignment.ChangeCalculator.Interfaces;
using CashRegisterAssignment.ChangeCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.Services
{
    public class ChangeCalculatorService: IChangeCalculatorService
    {
        (string Name, List<decimal> Denominations) activeCurrency;
        ICurrencyStore _currencyStore;       
        public bool isValid { get; set; }
        private List<ICashAmount> cashCollection = new List<ICashAmount>();
        private readonly IChangeCalculatorLogService<ChangeCalculator> _logService;
        public ChangeCalculatorService(ICurrencyStore currencyStore,
            IChangeCalculatorLogService<ChangeCalculator> logService)
        {
            _currencyStore = currencyStore;
            _logService = logService;
            isValid = false;
        }
        
        public void SetCurrencyStoreCollection(List<CurrencyItem> currencyCollection)
        {
            _currencyStore.SetStoreCurrenciesCollection(currencyCollection);
        }

        /// <summary>
        /// Sets the Currency that will be used in the application.
        /// This currency must be present in the Currency Store.
        /// </summary>
        /// <param name="activeCurrencyName"></param>
        /// <exception cref="CurrencyDataNotPresentException"></exception>
        public void SetActiveCurrency(string activeCurrencyName)
        {
            try
            {
                activeCurrency = (activeCurrencyName, _currencyStore.availableCurrencies[activeCurrencyName]);
            }catch(Exception ex)
            {
                _logService.AddLog(IChangeCalculatorLogService<ChangeCalculator>.Level.Error, "Unable to set active currency. See exception details. ", ex);
                throw new CurrencyDataNotPresentException($"Unable to set active currency. Error: {ex.Message}, {ex.InnerException}");
            }
            
        }

        /// <summary>
        /// Gets the Active Currency.
        /// It returns a Tuple containing the name of the currency and 
        /// the list of Denominations that the active currency can handle.
        /// </summary>
        /// <returns></returns>
        public (string Name, List<decimal> Denominations) GetActiveCurrency()
        {
            return activeCurrency;

        }

        /// <summary>
        /// A property that validates if the cash collection that the 
        /// user inputs has valid denominations for the active currency.
        /// </summary>
        public List<ICashAmount> CashCollection
        {
            get
            {
                return cashCollection;
            }
            set
            {
                foreach (ICashAmount item in value)
                {
                    if (!GetActiveCurrency().Denominations.Contains(item.Denomination))
                    {
                        isValid = false;
                        _logService.AddLog(IChangeCalculatorLogService<ChangeCalculator>.Level.Error, "Currency Denomination not found.");
                        throw new IncorrectMoneyInputException($"Invalid cash input {item.Denomination}");
                    }
                }
                isValid = true;
                cashCollection = value;
            }
        }


    }
}
