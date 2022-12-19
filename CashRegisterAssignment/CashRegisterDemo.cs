using CashRegisterAssignment.ChangeCalculator.Interfaces;
using CashRegisterAssignment.ChangeCalculator.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegisterAssignment
{
    /// <summary>
    /// This is the Demonstration Application.
    /// It contains two ready to use scenarios:
    /// 1.- Run the demo app with previously loaded data: Price List and Cash recieved for 
    ///     submitting.
    /// 2.- Step by step scenario for creating your own price list and your one cash collection.
    /// 
    /// 
    /// </summary>
    internal class CashRegisterDemo
    {
        IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        IChangeCalculator _changeCalculator;
        public CashRegisterDemo(IChangeCalculator changeCalculator)
        {
            _changeCalculator = changeCalculator; ;
        }

        /// <summary>
        /// This method starts the demonstration. 
        /// 1.- It reads the currency from the appsettings.json file.
        /// 2.- Sets the currency type in the application.
        /// 3.- Runs a demonstration of the routine.
        /// </summary>
        public void Run()
        {
            string currency = Configuration.GetValue<string>("ChangeCalculatorSettings:ActiveCurrency");
            List<KeyValuePair<string, List<decimal[]>>> currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<Dictionary<string, List<decimal[]>>>().ToList();

            List<decimal> priceList;
            List<ICashAmount> cashAmounts = new List<ICashAmount>();

            Console.WriteLine("<<Cash Register Assignment>>");
            Console.WriteLine($"The current currency config file is {currency}");
            Console.WriteLine("Note: The currency can be changed at the appsettings.json file");

            _changeCalculator.SetCurrencyStoreCollection(currenciesCollection);
            _changeCalculator.SetActiveCurrency(currency);
            Console.WriteLine($"The active currency is {_changeCalculator.GetActiveCurrency()} ");
            Console.WriteLine("These are the coins and bills accepted:");
            Console.WriteLine(_changeCalculator.GetAcceptedCoinsAndBills());

            Console.WriteLine("Testing out the SubmitCash Method. It requires a collection of 'Prices' and a collection of 'Quantities and Denominations' for paying the total price.");
            Console.WriteLine("1.- Please input the 'Prices' Input a price and press enter to input one by one. At the end press 'x' when done.");
            var i = 1;
            priceList = new List<decimal>();
            var res = String.Empty;
            int quantity = 0;
            do
            {
                Console.WriteLine($"Price {i}");
                res = Console.ReadLine();
                if (res != "x")
                {
                    bool isNumber = decimal.TryParse(res, out decimal price);
                    if (isNumber)
                    {
                        priceList.Add(price);
                    }
                }
                i++;
            } while (res != "x");
            Console.WriteLine("2.- Now please input the 'Quantity' and cash 'Denominations'. Input one by one. Press x when done. Example: Quantity 1, Denomination 20.00");
            i = 0;
            do
            {
                Console.WriteLine("Quantity: ");
                res = Console.ReadLine();
                if (res != "x")
                {
                    bool isNumber = int.TryParse(res, out int qty);
                    if (isNumber)
                    {
                        quantity = qty;
                    }
                    Console.WriteLine("Denomination: ");
                    res = Console.ReadLine();
                    if (res != "x")
                    {
                        bool isNumberDenomination = decimal.TryParse(res, out decimal denomination);
                        if (isNumberDenomination)
                        {
                            cashAmounts.Add(new CashAmount() { Quantity = quantity, Denomination = denomination });
                        }
                    }
                }
                i++;
            } while (res != "x");


            List<ICashAmount> result = _changeCalculator.SubmitCash(priceList, cashAmounts);

            Console.WriteLine("Cash Returned:");
            foreach (var item in result)
            {
                Console.WriteLine($"Quantity: {item.Quantity} - Denomination: {item.Denomination}");
            }
            Console.WriteLine();
            Console.WriteLine("<<Cash Register Assignment End>>");
        }

    }
}
