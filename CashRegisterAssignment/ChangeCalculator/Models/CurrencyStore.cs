using CashRegisterAssignment.ChangeCalculator.CustomExceptions;
using CashRegisterAssignment.ChangeCalculator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.Models
{
    /// <summary>
    /// This CurrencyStore could be allocated in a database server or 
    /// in a configuration file.
    /// This CurrencyStore is the place to add more denominations.
    /// </summary>
    public class CurrencyStore : ICurrencyStore
    {
        public Dictionary<string, List<decimal>> availableCurrencies { get; set; }

        /// <summary>
        /// Orders in descending order each of the Currency Denomination set and
        /// adds the Currency set to the Available Catalog of Currencies.
        /// </summary>
        /// <param name="currenciesCollection"></param>
        /// <exception cref="CurrencyDataNotPresentException"></exception>
        public void SetStoreCurrenciesCollection(List<CurrencyItem> currenciesCollection)
        {
            try
            {
                availableCurrencies = new Dictionary<string, List<decimal>>();
                foreach (CurrencyItem item in currenciesCollection)
                {
                    availableCurrencies.Add(item.CurrencyName, item.CurrencyDenominations.OrderByDescending(x => x).ToList());
                }

            }
            catch (Exception ex)
            {
                throw new CurrencyDataNotPresentException($"The currency is not available or is not in the correct format. {ex.Message}");
            }
        }

    }
}
