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
        public void SetCurrencyStoreCollection(List<CurrencyItem> currenciesCollection);
        public void SetActiveCurrency(string activeCurrency);
        public string GetActiveCurrency();
        public string GetAcceptedCoinsAndBills();

        public ICashWrapper SubmitCash(List<decimal> ItemsPrices, List<ICashAmount> cashAmounts);


    }
}
