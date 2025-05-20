using Microsoft.AspNetCore.Mvc;
using WebhookSheets.DataTransfer.PlanilhaFinanceiro.Requests;

namespace WebhookSheets.Controller.PlanilhaFinanceiro
{
    [Route("api/financeiro")]
    [ApiController]
    public class PlanilhaFinanceiroController : ControllerBase
    {
        public PlanilhaFinanceiroController()
        {
        }

        [HttpPost]
        public ActionResult ReceberAlteracaoPlanilhaFinanceiro([FromBody] GoogleSheetFinanceiroRequest request)
        {
            Console.WriteLine("testando", request);

            Console.WriteLine($"Recebido evento: {request.Evento}");
            Console.WriteLine($"Aba: {request.Sheet}, Linha: {request.Row}");
            Console.WriteLine($"Data/Hora: {request.Time}");
            Console.WriteLine($"Valores: {request.Values}");


            return Ok();
        }
    }
}
