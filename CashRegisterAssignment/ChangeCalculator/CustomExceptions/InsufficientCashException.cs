using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.CustomExceptions
{
    /// <summary>
    /// Custom Exception for managing insufficient cash being 
    /// recieved in a Submit.
    /// </summary>
    public class InsufficientCashException : Exception
    {
        public InsufficientCashException()
        {
        }

        public InsufficientCashException(string message)
            : base(message)
        {
        }
    }
}
