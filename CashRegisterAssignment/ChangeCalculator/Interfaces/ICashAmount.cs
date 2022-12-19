using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.Interfaces
{
    /// <summary>
    /// Interface for manipulating the cash data structure.
    /// </summary>
    public interface ICashAmount
    {
        public int Quantity { get; set; }
        public decimal Denomination { get; set; }
    }
}
