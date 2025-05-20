using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace WebhookSheets.Service.Mensageria.RabbitMqService
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMqService()
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq-3dkk.onrender.com",
                Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "admin",
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? " FinanceiroHoot!23"
            };

        }

        public async Task ConexaoAsync()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "hello",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            const string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "hello",
                body: body);

            Console.WriteLine($" [x] Sent {message}");
        }
        public async Task SendMessage(object mensagem, string fila = "planilha-financeiro", CancellationToken cancellation = default)
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
