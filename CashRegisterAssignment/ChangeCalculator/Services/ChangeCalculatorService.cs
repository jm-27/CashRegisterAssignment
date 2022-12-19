using CashRegisterAssignment.ChangeCalculator.CustomExceptions;
using CashRegisterAssignment.ChangeCalculator.Interfaces;
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
        private List<ICashAmount> cashCollection;
        public ChangeCalculatorService(ICurrencyStore currencyStore)
        {
            _currencyStore = currencyStore;
            isValid = false;
        }
        
        public void SetCurrencyStoreCollection(List<KeyValuePair<string, List<decimal[]>>> currencyCollection)
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
                        throw new IncorrectMoneyInputException($"Invalid cash input {item.Denomination}");
                    }
                }
                isValid = true;
                cashCollection = value;
            }
        }


    }
}
