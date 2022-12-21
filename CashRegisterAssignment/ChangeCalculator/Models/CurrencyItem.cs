using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.Models
{
    /// <summary>
    /// For use with the appsettings.json configuration section: CurrenciesCollection.
    /// It represents the structure read at the Configuration.GetSection("CurrenciesCollection") and it is 
    /// used for retrieving the list of currencies available.
    /// </summary>
    public class CurrencyItem
    {
        public string CurrencyName { get; set; }
        public decimal[] CurrencyDenominations { get; set; }
    }
}
