using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.Interfaces
{
    public interface IChangeCalculatorLogService<T>
    {
        public enum Level{
            Info,
            Warning,
            Error,
            Critical
        }
        public void AddLog(Level level, string message, Exception? exception = null);
    }
}
