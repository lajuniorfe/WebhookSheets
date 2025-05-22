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
        private IConnection? _connection;
        private IChannel? _channel;


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

        private async Task AbrirConexao()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = await _connectionFactory.CreateConnectionAsync();
            }

            if (_channel == null || !_channel.IsOpen)
            {
                _channel = await _connection.CreateChannelAsync();
            }
        }

        public async Task SendMessage(GoogleSheetFinanceiroRequest mensagem, string fila = "planilha-financeiro", CancellationToken cancellation = default)
        {
            try
            {
               await AbrirConexao();

                await _channel.QueueDeclareAsync(queue: fila, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mensagem));

                await _channel.BasicPublishAsync(exchange: "", routingKey: fila, body: body, cancellation);

                Console.WriteLine($"Mensagem publicada na fila '{fila}'.");

                await Task.Delay(Timeout.Infinite, cancellation);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
