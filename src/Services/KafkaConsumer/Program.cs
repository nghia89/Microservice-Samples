using Confluent.Kafka;
using KafkaConsumer;
using KafkaConsumer.Extensions;
using KafkaConsumer.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddElasticsearch(hostContext.Configuration);
        //services.AddHostedService<KafkaConsumerHandlerTest>();
        services.AddHostedService<KafkaConsumerHandler>();
        services.AddSingleton<ConsumerConfig>(option =>
        {
            ConsumerConfig config = new ConsumerConfig();
            config.GroupId = "st_consumer_group_product";
            config.BootstrapServers = "localhost:9092";
            config.AutoOffsetReset = AutoOffsetReset.Earliest;
            return config;
        });
        services.AddSingleton<IProductService, ProductService>();
        //services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

