using CashRegisterAssignment.ChangeCalculator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment.ChangeCalculator.Models
{
    /// <summary>
    /// Data Structure (Model) for manipulating the cash
    /// the customer gives or recieves.
    /// </summary>
    public class CashAmount: ICashAmount
    {
        public int Quantity { get; set; }
        public decimal Denomination { get; set; }
    }
}
