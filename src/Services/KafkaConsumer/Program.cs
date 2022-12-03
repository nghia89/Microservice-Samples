using KafkaConsumer;
using KafkaConsumer.Extensions;
using KafkaConsumer.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddElasticsearch(hostContext.Configuration);
        services.AddSingleton<IHostedService, KafkaConsumerHandler>();
        services.AddSingleton<IProductService, ProductService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

