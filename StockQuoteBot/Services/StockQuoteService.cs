using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using System.Text.Json;
using StockQuoteBot.Models;
using ChatApp.Data.Models;

namespace StockQuoteBot.Services
{
    public class StockQuoteService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public StockQuoteService(IOptions<RabbitMQSettings> options)
        {
            var factory = new ConnectionFactory() { HostName = options.Value.Host };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = options.Value.Queue;
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockRequest = JsonSerializer.Deserialize<StockQuoteRequest>(message);


                var quote = await FetchStockQuote(stockRequest.StockCode);
                var formattedMessage = FormatQuoteMessage(stockRequest.StockCode, quote);

                //TODO: Simulate sending this back to the chat application via another queue or service
                SendMessage(formattedMessage, stockRequest);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            await Task.CompletedTask;
        }

        private async void SendMessage(string formattedMessage, StockQuoteRequest stockRequest)
        {
            try
            {
                var client = new HttpClient();

                var url = "http://localhost:5169/api/Chat/Messages";

                var messageBody = new ChatMessage()
                {
                    ChatRoomId = stockRequest.ChatRoomId,
                    Content = formattedMessage,
                    Timestamp = DateTime.UtcNow
                };

                HttpContent postBody = new StringContent(JsonSerializer.Serialize(messageBody), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, postBody);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception occured: {ex.Message}");//Improve logging here
            }
        }

        private async Task<string> FetchStockQuote(string stockCode)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
            var lines = response.Split('\n');
            var data = lines[1].Split(',');
            return data[6]; // Assuming the 7th column, the closing price, is the correct one
        }

        private string FormatQuoteMessage(string stockCode, string quote)
        {
            return $"{stockCode.ToUpper()} quote is {quote} per share";
        }
    }
}
