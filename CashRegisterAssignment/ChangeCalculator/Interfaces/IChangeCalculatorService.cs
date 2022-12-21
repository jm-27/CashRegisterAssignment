using CashRegisterAssignment.ChangeCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.Interfaces
{
    /// <summary>
    /// Interface for Change Calculator Services.
    /// </summary>
    public interface IChangeCalculatorService
    {
        public List<ICashAmount> CashCollection { get; set; }
        public bool isValid { get; set; }
        public void SetCurrencyStoreCollection(List<CurrencyItem> currencyCollection);
        public void SetActiveCurrency(string activeCurrencyName);
        public (string Name, List<decimal> Denominations) GetActiveCurrency();

    }
}
