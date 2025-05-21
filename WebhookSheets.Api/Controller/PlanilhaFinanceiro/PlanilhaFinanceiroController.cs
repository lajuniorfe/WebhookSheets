using Microsoft.AspNetCore.Mvc;
using WebhookSheets.DataTransfer.PlanilhaFinanceiro.Requests;
using WebhookSheets.Service.Mensageria.RabbitMqService;

namespace WebhookSheets.Controller.PlanilhaFinanceiro
{
    [Route("api/financeiro")]
    [ApiController]
    public class PlanilhaFinanceiroController : ControllerBase
    {
        private readonly IRabbitMqService _rabbit;

        public PlanilhaFinanceiroController(IRabbitMqService rabbit)
        {
            _rabbit = rabbit;
        }

        [HttpPost]
        public async Task<ActionResult> ReceberAlteracaoPlanilhaFinanceiro([FromBody] GoogleSheetFinanceiroRequest request)
        {
            Console.WriteLine($"Recebido evento: {request.Evento}");
            Console.WriteLine($"Aba: {request.Sheet}, Linha: {request.Row}");
            Console.WriteLine($"Data/Hora: {request.Time}");
            foreach (var i in request.Values)
            {
                Console.WriteLine($"Valores: {i}");

            }

            await _rabbit.SendMessage(request, "planilha-financeiro");

            return Ok();
        }
    }
}
