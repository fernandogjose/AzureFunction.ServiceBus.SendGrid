using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunction.ServiceBus.SendGrid.FunctionApp
{
    public static class EmailNovoUsuarioFunction
    {
        [FunctionName("EmailNovoUsuarioFunction")]
        public static void Run([ServiceBusTrigger("email-novousuario", Connection = "Endpoint=sb://servicebus-sendgrid.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xdZlo839YKVT+f+eA3w1TsgRHuPW9/sj27OFcyyLn3o=")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
