using System;
using System.Text.Json;
using Confluent.Kafka;
using Nest;
using Shared.DTOs.Product;

namespace KafkaConsumer
{
    public class KafkaConsumerHandler : IHostedService
    {
        private readonly IElasticClient _elasticClient;
        private readonly string topic = "product";
        public KafkaConsumerHandler(
                       IElasticClient elasticClient
                       )
        {
            _elasticClient = elasticClient;
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

                        // Index product dto
                        await _elasticClient.IndexDocumentAsync(product);
                        var result = await _elasticClient.SearchAsync<CreateProductDto>(
                       s => s.Query(
                           q => q.QueryString(
                               d => d.Query('*' + "string" + '*')
                           )).Size(5000));

                        Console.WriteLine("ProductsController Get - ", DateTime.UtcNow);
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

