using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using ChatApp.Data.Models;

namespace ChatApp.Services
{
    public class RabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQService(IOptions<RabbitMQSettings> options)
        {
            var factory = new ConnectionFactory() { HostName = options.Value.Host };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = options.Value.Queue;
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void PublishStockRequest(string stockCode)
        {
            var message = new StockQuoteRequest { StockCode = stockCode };
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        }
    }
}
