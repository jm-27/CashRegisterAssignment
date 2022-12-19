using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.CustomExceptions
{
    /// <summary>
    /// Custom Exception for handling no currency in storage.
    /// </summary>
    public class CurrencyDataNotPresentException : Exception
    {
        public CurrencyDataNotPresentException()
        {
        }

        public CurrencyDataNotPresentException(string message)
            : base(message)
        {
        }
    }
}
