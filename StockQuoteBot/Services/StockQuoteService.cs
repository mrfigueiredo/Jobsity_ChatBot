using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;
using StockQuoteBot.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace StockQuoteBot.Services
{
    public class StockQuoteService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly HubConnection _hub;

        public StockQuoteService(IOptions<RabbitMQSettings> options)
        {
            var factory = new ConnectionFactory() { HostName = options.Value.Host };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = options.Value.Queue;
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5169/chathub")
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ConnectWithRetryAsync();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockRequest = JsonSerializer.Deserialize<StockQuoteRequest>(message);


                var quote = await FetchStockQuote(stockRequest.StockCode);
                var formattedMessage = FormatQuoteMessage(stockRequest.StockCode, quote);

                //TODO: Simulate sending this back to the chat application via another queue or service
                await _hub.InvokeAsync("SendMessage", "StockBot", formattedMessage, stockRequest.ChatRoom);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            await Task.CompletedTask;
        }

        private async Task ConnectWithRetryAsync()
        {
            while (true)
            {
                try
                {
                    await _hub.StartAsync();
                    break;
                }
                catch
                {
                    await Task.Delay(5000); // Wait 5 seconds before retrying
                }
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
