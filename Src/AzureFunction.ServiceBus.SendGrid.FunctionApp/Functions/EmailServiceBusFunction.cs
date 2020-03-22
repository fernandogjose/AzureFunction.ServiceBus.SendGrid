using AzureFunction.ServiceBus.SendGrid.FunctionApp.Interfaces.Services;
using AzureFunction.ServiceBus.SendGrid.FunctionApp.Models;
using AzureFunction.ServiceBus.SendGrid.FunctionApp.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AzureFunction.ServiceBus.SendGrid.FunctionApp.Functions
{
    public static class EmailServiceBusFunction
    {
        private static IEnumerable<IEmailEnviarService> _emailEnviarServices;

        [FunctionName("EmailServiceBusFunction")]
        public static void Run([ServiceBusTrigger("email-enviar", Connection = "AzureWebJobsServiceBus")]string emailQueue, ILogger log)
        {
            log.LogInformation("Inicio do processamento");
            RegisterServices();
            Processar(emailQueue, log);
            log.LogInformation("Fim do processamento");
        }

        private static void RegisterServices()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IEmailEnviarService, NovoUsuarioEmailService>();
            serviceCollection.AddTransient<IEmailEnviarService, EsqueciMinhaSenhaEmailService>();

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            _emailEnviarServices = serviceProvider.GetServices<IEmailEnviarService>();
        }

        private static void Processar(string emailQueue, ILogger log)
        {
            // Mapper
            EmailEnviar requestEmailEnviar = JsonConvert.DeserializeObject<EmailEnviar>(emailQueue);
            if (requestEmailEnviar == null || string.IsNullOrEmpty(requestEmailEnviar.Tipo))
            {
                log.LogInformation("Tipo de e-mail inválido");
                return;
            }

            // Busca o tipo
            IEmailEnviarService emailEnviarService = _emailEnviarServices.FirstOrDefault(x => x.Tipo == requestEmailEnviar.Tipo);
            if (emailEnviarService == null)
            {
                log.LogInformation($"Serviço não encontrado para o tipo de e-mail {requestEmailEnviar.Tipo}");
                return;
            }

            // Envia e-mail
            emailEnviarService.Enviar(requestEmailEnviar);
        }
    }
}
