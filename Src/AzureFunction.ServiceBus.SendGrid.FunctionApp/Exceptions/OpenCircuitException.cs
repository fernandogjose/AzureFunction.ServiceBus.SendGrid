using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunction.ServiceBus.SendGrid.FunctionApp.Exceptions
{
    public class OpenCircuitException : Exception
    {
        public OpenCircuitException(string message) : base(message)
        {

        }
    }

    public class CircuitBreakerOperationException : Exception
    {
        public CircuitBreakerOperationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
