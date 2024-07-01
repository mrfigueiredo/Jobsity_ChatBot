using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockQuoteBot.Models;
using StockQuoteBot.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<RabbitMQSettings>(context.Configuration.GetSection("RabbitMQ"));
        services.AddHostedService<StockQuoteService>();
    })
    .Build();

await host.RunAsync();