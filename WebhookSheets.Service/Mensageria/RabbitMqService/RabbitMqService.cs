using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using WebhookSheets.DataTransfer.PlanilhaFinanceiro.Requests;

namespace WebhookSheets.Service.Mensageria.RabbitMqService
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;


        public RabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"],
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"]
            };
         
        }

        public async Task SendMessage(GoogleSheetFinanceiroRequest mensagem, string fila = "planilha-financeiro", CancellationToken cancellation = default)
        {
            try
            {
                var connection = await _connectionFactory.CreateConnectionAsync();
                var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: fila, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mensagem));

                await channel.BasicPublishAsync(exchange: "", routingKey: fila, body: body, cancellation);

                Console.WriteLine($"Mensagem publicada na fila '{fila}'.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
