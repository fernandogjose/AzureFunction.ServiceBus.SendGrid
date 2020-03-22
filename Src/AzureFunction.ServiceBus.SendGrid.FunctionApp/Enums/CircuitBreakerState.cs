using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunction.ServiceBus.SendGrid.FunctionApp.Enums
{
    public enum CircuitBreakerState
    {
        Closed,
        Open,
        HalfOpen
    }
}
