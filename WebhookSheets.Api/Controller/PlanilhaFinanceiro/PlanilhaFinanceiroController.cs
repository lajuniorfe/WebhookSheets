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
            try
            {
                await _rabbit.SendMessage(request, "planilha-financeiro");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
