using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebhookSheets.DataTransfer.PlanilhaFinanceiro.Requests
{
    public class GoogleSheetFinanceiroRequest
    {
        public string Evento { get; set; } // "FORM_SUBMIT"
        public string Sheet { get; set; }  // Nome da aba
        public int Row { get; set; }       // Número da linha inserida
        public List<string> Values { get; set; } // Dados da linha
        public Dictionary<string, List<string>> Named { get; set; } // Pergunta → [resposta]
        public DateTime Time { get; set; }  // Timestamp ISO 8601
    }
}
