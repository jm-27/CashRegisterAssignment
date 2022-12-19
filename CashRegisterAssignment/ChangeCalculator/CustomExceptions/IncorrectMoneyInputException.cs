using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.CustomExceptions
{
    /// <summary>
    /// Custom Exception for handling incorrect money input.
    /// </summary>
    public class IncorrectMoneyInputException : Exception
    {
        public IncorrectMoneyInputException()
        {
        }

        public IncorrectMoneyInputException(string message)
            : base(message)
        {
        }
    }
}
