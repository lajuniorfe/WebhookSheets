using WebhookSheets.DataTransfer.PlanilhaFinanceiro.Requests;

namespace WebhookSheets.Service.Mensageria.RabbitMqService
{
    public interface IRabbitMqService
    {
        Task SendMessage(GoogleSheetFinanceiroRequest mensagem, string fila = "planilha-financeiro", CancellationToken cancellation = default);

    }
}
