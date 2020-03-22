using AzureFunction.ServiceBus.SendGrid.FunctionApp.Interfaces.Services;
using System;

namespace AzureFunction.ServiceBus.SendGrid.FunctionApp.Services
{
    public class EsqueciMinhaSenhaEmailService : IEmailEnviarService
    {
        public string Tipo => "EsqueciMinhaSenha";

        public void Enviar(Models.EmailEnviar request)
        {
            throw new NotImplementedException();
        }
    }
}
