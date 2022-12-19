using CashRegisterAssignment.ChangeCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.Interfaces
{
    /// <summary>
    /// Interface Change Calulator Operations.
    /// </summary>
    public interface IChangeCalculator
    {
        public void SetCurrencyStoreCollection(List<KeyValuePair<string, List<decimal[]>>> currenciesCollection);
        public void SetActiveCurrency(string activeCurrency);
        public string GetActiveCurrency();
        public string GetAcceptedCoinsAndBills();

        public List<ICashAmount> SubmitCash(List<decimal> ItemsPrices, List<ICashAmount> cashAmounts);


    }
}
