using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Sender
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://servicebus-sendgrid.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xLm9Pi0XfzUad76aLeO0u6MZ4xm7g29HPIo0gdJA70Q=";
        const string QueueName = "email-enviar";
        static IQueueClient queueClient;

        public static async Task Main(string[] args)
        {
            // Config do service bus
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            // Envia a mensagem para a fila
            await SendMessagesAsync().ConfigureAwait(true);

            // Finaliza
            Console.ReadKey();
            await queueClient.CloseAsync();
        }

        static async Task SendMessagesAsync()
        {
            try
            {
                // Criar uma mensagem para enviar
                EmailEnviar emailEnviar = new EmailEnviar
                {
                    Assunto = "teste",
                    Tipo = "NovoUsuario",
                    Para = new Dictionary<string, string> { { "Fernando", "fernandogjose@gmail.com" } },
                    Parametros = new Dictionary<string, string> {
                        { "Nome", "Priscila Antunes" },
                        { "link", "http://fernandojose.com.br" },
                        { "login", "p.antunes" }
                    }
                };
                string messageBody = JsonConvert.SerializeObject(emailEnviar);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                // Mostra a mensagem no console
                Console.WriteLine($"Sending message: {messageBody}");

                // Envia a mensagem para a fila
                await queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }

    public class EmailEnviar
    {
        public string Tipo { get; set; }

        public string Assunto { get; set; }

        public Dictionary<string, string> Para { get; set; }

        public Dictionary<string, string> Parametros { get; set; }
    }
}