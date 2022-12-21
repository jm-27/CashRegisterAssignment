using CashRegisterAssignment.ChangeCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.Interfaces
{
    public interface ICashWrapper
    {
        public enum Status
        {
            Initial,
            Valid,
            InsufficientCash,
            InvalidInput,
            Invalid

        }
        public string Currency { get; set; }
        public List<ICashAmount>? CashAmount { get; set; }
        public ICashWrapper SetCashWrapperData(ICashWrapper.Status Status, string Currency, List<ICashAmount> CashAmount);
    }
}
