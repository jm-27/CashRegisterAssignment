using CashRegisterAssignment.ChangeCalculator.Interfaces;
using Microsoft.Extensions.Logging;


namespace CashRegisterAssignment.ChangeCalculator.Services
{
    /// <summary>
    /// Logging Service. It Allows to Add a Log with a specified level.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChangeCalculatorLogService<T> : IChangeCalculatorLogService<T>
    {
        private ILogger<T> logger;
        public ChangeCalculatorLogService(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public void AddLog(IChangeCalculatorLogService<T>.Level level, string message, Exception? exception = null)
        {
            switch (level)
            {
                case IChangeCalculatorLogService<T>.Level.Info:
                    {
                        logger.LogInformation(exception, message, DateTime.UtcNow.ToLongTimeString());
                        break;
                    }
                case IChangeCalculatorLogService<T>.Level.Warning:
                    logger.LogWarning(exception, message, DateTime.UtcNow.ToLongTimeString());
                    break;
                case IChangeCalculatorLogService<T>.Level.Error:
                    logger.LogError(exception, message, DateTime.UtcNow.ToLongTimeString());
                    break;
                case IChangeCalculatorLogService<T>.Level.Critical:
                    logger.LogCritical(exception, message, DateTime.UtcNow.ToLongTimeString());
                    break;
                default:
                    {
                        break;
                    }
            }

        }
    }
}
