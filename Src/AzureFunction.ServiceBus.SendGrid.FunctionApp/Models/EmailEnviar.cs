using System.Collections.Generic;

namespace AzureFunction.ServiceBus.SendGrid.FunctionApp.Models
{
    public class EmailEnviar
    {
        public string Tipo { get; set; }

        public string Assunto { get; set; }

        public Dictionary<string, string> Para { get; set; }

        public Dictionary<string, string> Parametros { get; set; }
    }
}
