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

        public void SetStoreCurrenciesCollection(List<KeyValuePair<string, List<decimal[]>>> currenciesCollection)
        {
            try
            {
                availableCurrencies = new Dictionary<string, List<decimal>>();
                foreach (KeyValuePair<string, List<decimal[]>> item in currenciesCollection)
                {
                    availableCurrencies.Add(item.Key, item.Value.FirstOrDefault().OrderByDescending(x => x).ToList());
                }

            }
            catch (Exception ex)
            {
                throw new CurrencyDataNotPresentException($"The currency is not available or is not in the correct format. {ex.Message}");
            }
        }

    }
}
