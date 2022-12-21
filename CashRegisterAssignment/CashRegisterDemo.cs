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
    /// It contains a ready to use scenario:
    /// 1.- Run the demo app step by step for creating your own price list and your one cash collection.
    /// 2.- Input your price list and press x to continue
    /// 3.- Input your cash amount and press x to continue
    /// 4.- The app will process the submitted prices and cash collection
    /// 5.- The app will return a result based on the input taken.
    /// It includes Microsoft Extension Logging + SeriLog for logging messages and errors into console and into text file.
    /// Text file can be found in the bin folder under the name logs.txt.
    /// </summary>
    internal class CashRegisterDemo
    {
        IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        IChangeCalculator _changeCalculator;
        ICashAmount _cashAmount;
        public CashRegisterDemo(IChangeCalculator changeCalculator, ICashAmount cashAmount)
        {
            _changeCalculator = changeCalculator; 
            _cashAmount = cashAmount;
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
            List<CurrencyItem> currenciesCollection = Configuration.GetSection("CurrenciesCollection").Get<List<CurrencyItem>>();
            
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
                    else
                    {
                        Console.WriteLine("Not a valid input. Price must be an integer or a decimal. Try again.");
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
                    else
                    {
                        Console.WriteLine("Not a valid input. Quantity must be an integer. Try again.");
                    }
                    Console.WriteLine("Denomination: ");
                    res = Console.ReadLine();
                    if (res != "x")
                    {
                        bool isNumberDenomination = decimal.TryParse(res, out decimal denomination);
                        if (isNumberDenomination)
                        {
                            cashAmounts.Add(_cashAmount.SetCashAmountData(quantity, denomination));
                        }
                        else
                        {
                            Console.WriteLine("Not a valid input. Denomination must be an integer or a decimal. Try again.");
                        }
                    }
                }
                i++;
            } while (res != "x");


            ICashWrapper result = _changeCalculator.SubmitCash(priceList, cashAmounts);

            switch (((CashWrapper)result).Status)
            {
                case ICashWrapper.Status.Valid:
                    {
                        Console.WriteLine("Cash Returned:");
                        foreach (var item in result.CashAmount)
                        {
                            Console.WriteLine($"Quantity: {item.Quantity} - Denomination: {item.Denomination}");
                        }
                        break;
                    }
                case ICashWrapper.Status.InsufficientCash:
                    {
                        Console.WriteLine("Not Enough Cash For This Operation.");                        
                        break;
                    }
                case ICashWrapper.Status.InvalidInput:
                    {
                        Console.WriteLine("Invalid Input: Invalid Denomination.");
                        break;
                    }
                default:
                    {
                        break;
                    }

            }

            Console.WriteLine();
            Console.WriteLine("<<Cash Register Assignment End>>");
        }

    }
}
