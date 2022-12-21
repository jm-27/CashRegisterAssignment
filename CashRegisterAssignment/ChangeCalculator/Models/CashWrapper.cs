using CashRegisterAssignment.ChangeCalculator.Interfaces;
using System.ComponentModel;

namespace CashRegisterAssignment.ChangeCalculator.Models
{
    /// <summary>
    /// Class Wrapper for storing the Cash Amount. It informs if the 
    /// Cash Amount is valid or not. It informs the kind of currency
    /// the Cash Amount uses.
    /// </summary>
    public class CashWrapper:ICashWrapper
    {

        public ICashWrapper.Status Status { get; set; }
        public string Currency { get; set; }
        public List<ICashAmount>? CashAmount { get; set; }

        public CashWrapper(ICashWrapper.Status Status, string Currency, List<ICashAmount>? CashAmount)
        {
            this.Status = Status;
            this.Currency = Currency;
            this.CashAmount = CashAmount;
        }

        /// <summary>
        /// Returns a new instance of Cash Wrapper.
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Currency"></param>
        /// <param name="CashAmount"></param>
        /// <returns></returns>
        public ICashWrapper SetCashWrapperData(ICashWrapper.Status Status, string Currency, List<ICashAmount> CashAmount)
        {
            return new CashWrapper(Status, Currency, CashAmount);
        }
    }
}
