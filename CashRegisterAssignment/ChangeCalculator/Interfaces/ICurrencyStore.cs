using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.Interfaces
{
    /// <summary>
    /// Interface for Currency Storage.
    /// </summary>
    public interface ICurrencyStore
    {
        public Dictionary<string, List<decimal>> availableCurrencies { get;  }
        public void SetStoreCurrenciesCollection(List<KeyValuePair<string, List<decimal[]>>> currenciesCollection);
    }
}
