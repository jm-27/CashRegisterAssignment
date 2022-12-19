using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashRegisterAssignment.ChangeCalculator.Interfaces;
using CashRegisterAssignment.ChangeCalculator.Models;
using CashRegisterAssignment.ChangeCalculator;
using Microsoft.Extensions.DependencyInjection;
using CashRegisterAssignment.ChangeCalculator.Services;

namespace CashRegisterAssignmentTests
{
    public class TestFixtures
    {
        public ServiceProvider serviceProvider { get; set; }
        public TestFixtures() {
            var services = new ServiceCollection();
            services.AddTransient<IChangeCalculatorService, ChangeCalculatorService>();
            services.AddTransient<ICurrencyStore, CurrencyStore>();
            services.AddTransient<IChangeCalculator, ChangeCalculator>();
            serviceProvider = services.BuildServiceProvider(true);

        }   
    }

}
