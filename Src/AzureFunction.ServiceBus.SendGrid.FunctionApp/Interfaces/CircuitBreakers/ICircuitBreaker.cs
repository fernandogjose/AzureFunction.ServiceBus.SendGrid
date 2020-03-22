using AzureFunction.ServiceBus.SendGrid.FunctionApp.Enums;
using System;

namespace AzureFunction.ServiceBus.SendGrid.FunctionApp.Interfaces.CircuitBreakers
{
    public interface ICircuitBreaker
    {
        CircuitBreakerState State { get; }
        void Reset();
        void Execute(Action action);
        bool IsClosed { get; }
        bool IsOpen { get; }
    }
}
