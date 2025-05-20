using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebhookSheets.Service.Mensageria.RabbitMqService
{
    public interface IRabbitMqService
    {
        Task ConexaoAsync();
        Task SendMessage(object mensagem, string fila = "planilha-financeiro", CancellationToken cancellation = default);

    }
}
