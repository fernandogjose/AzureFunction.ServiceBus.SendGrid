using AzureFunction.ServiceBus.SendGrid.FunctionApp.Interfaces.Services;
using System;

namespace AzureFunction.ServiceBus.SendGrid.FunctionApp.Services
{
    public class NovoUsuarioEmailService : IEmailEnviarService
    {
        public string Tipo => "NovoUsuario";

        public void Enviar(Models.EmailEnviar request)
        {
            throw new NotImplementedException();
        }
    }
}
