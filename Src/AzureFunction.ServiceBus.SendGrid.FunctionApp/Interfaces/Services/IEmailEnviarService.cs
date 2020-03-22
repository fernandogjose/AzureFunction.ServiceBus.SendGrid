using AzureFunction.ServiceBus.SendGrid.FunctionApp.Models;

namespace AzureFunction.ServiceBus.SendGrid.FunctionApp.Interfaces.Services
{
    public interface IEmailEnviarService
    {
        string Tipo { get; }

        void Enviar(EmailEnviar request);
    }
}
