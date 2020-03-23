using AzureFunction.ServiceBus.SendGrid.FunctionApp.Enums;
using AzureFunction.ServiceBus.SendGrid.FunctionApp.Exceptions;
using AzureFunction.ServiceBus.SendGrid.FunctionApp.Interfaces.CircuitBreakers;
using System;
using System.Timers;

namespace AzureFunction.ServiceBus.SendGrid.FunctionApp.CircuitBreakers
{
    public class CircuitBreaker : ICircuitBreaker
    {

        public event EventHandler StateChanged;

        private object monitor = new object();

        public int Timeout { get; private set; }

        public int Threshold { get; private set; }

        public CircuitBreakerState State { get; private set; }

        private Timer Timer { get; set; }

        private int FailureCount { get; set; }

        private Action Action { get; set; }

        public bool IsClosed { get { return State == CircuitBreakerState.Closed; } }

        public bool IsOpen { get { return State == CircuitBreakerState.Open; } }

        public CircuitBreaker(int threshold = 5, int timeout = 60000)
        {
            if (threshold <= 0)
                throw new ArgumentOutOfRangeException($"{threshold} deve ser maior que zero");

            if (timeout <= 0)
                throw new ArgumentOutOfRangeException($"{timeout} deve ser maior que zero");

            Threshold = threshold;
            Timeout = timeout;
            State = CircuitBreakerState.Closed;

            Timer = new Timer(timeout)
            {
                Enabled = false
            };
            Timer.Elapsed += Timer_Elapsed;
        }

        public void Execute(Action action)
        {
            if (State == CircuitBreakerState.Open)
            {
                throw new OpenCircuitException("Circuit breaker está aberto");
            }

            lock (monitor)
            {
                try
                {
                    Action = action;
                    Action();
                }
                catch (Exception ex)
                {
                    if (State == CircuitBreakerState.HalfOpen)
                    {
                        Trip();
                    }
                    else if (FailureCount <= Threshold)
                    {
                        FailureCount++;

                        //Ativa o Retry
                        if (Timer.Enabled == false)
                            Timer.Enabled = true;
                    }
                    else if (FailureCount >= Threshold)
                    {
                        Trip();
                    }

                    throw new CircuitBreakerOperationException("Envio de e-mail falhou", ex);
                }

                if (State == CircuitBreakerState.HalfOpen)
                {
                    Reset();
                }

                if (FailureCount > 0)
                {
                    FailureCount--;
                }
            }
        }

        public void Reset()
        {
            if (State != CircuitBreakerState.Closed)
            {
                ChangeState(CircuitBreakerState.Closed);
                Timer.Stop();
            }
        }

        private void Trip()
        {
            if (State != CircuitBreakerState.Open)
            {
                ChangeState(CircuitBreakerState.Open);
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (monitor)
            {
                try
                {
                    Execute(Action);
                    Reset();
                }
                catch
                {
                    if (FailureCount > Threshold)
                    {
                        Trip();

                        Timer.Elapsed -= Timer_Elapsed;
                        Timer.Enabled = false;
                        Timer.Stop();

                    }
                }
            }
        }

        private void ChangeState(CircuitBreakerState state)
        {
            State = state;
            OnCircuitBreakerStateChanged(new EventArgs() { });
        }

        private void OnCircuitBreakerStateChanged(EventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }
    }
}
