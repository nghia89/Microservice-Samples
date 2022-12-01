using KafkaConsumer;
using KafkaConsumer.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddElasticsearch(hostContext.Configuration);
        services.AddSingleton<IHostedService, KafkaConsumerHandler>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

