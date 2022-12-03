using System;
using System.Text.Json;
using Confluent.Kafka;
using KafkaConsumer.Services;
using Nest;
using Shared.DTOs.Product;

namespace KafkaConsumer
{
    public class KafkaConsumerHandler : IHostedService
    {
        private readonly IElasticClient _elasticClient;
        private readonly IProductService _productService;
        private readonly string topic = "product";
        public KafkaConsumerHandler(
                       IElasticClient elasticClient,
                       IProductService productService)
        {
            _elasticClient = elasticClient;
            _productService = productService;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var conf = new ConsumerConfig
            {
                GroupId = "st_consumer_group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf).Build())
            {
                builder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (true)
                    {
                        var consumer = builder.Consume(cancelToken.Token);
                        var value = consumer.Message.Value;
                        var product = JsonSerializer.Deserialize<CreateProductDto>(value);

                        if (product != null)
                            await _productService.SaveSingleAsync(product);
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.Topic} - {consumer.Partition.Value}- {consumer.Offset}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    builder.Close();
                }
            }

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}

