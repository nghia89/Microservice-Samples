using System;
using System.Text.Json;
using Confluent.Kafka;
using KafkaConsumer.Services;
using Nest;
using Shared.DTOs.Product;

namespace KafkaConsumer
{
    public class KafkaConsumerHandlerTest : IHostedService
    {
        private readonly IElasticClient _elasticClient;
        private readonly IProductService _productService;
        private readonly string _topic = "product01";
        private readonly IConsumer<Ignore, string> _consumer;
        private ConsumerConfig _config;
        public KafkaConsumerHandlerTest(
                       IElasticClient elasticClient,
                       IProductService productService,
                       ConsumerConfig config)
        {
            _elasticClient = elasticClient;
            _productService = productService;
            _config = config;
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            _consumer.Subscribe(_topic);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.CancelKeyPress += (_, e) =>
           {
               e.Cancel = true;
           };

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("Consumindo novas mensagens...");
                    var consumeResult = _consumer.Consume(cancellationToken);
                    throw new InvalidOperationException();
                    _consumer.Commit();
                    Console.WriteLine(
                        $"Consumed message '{consumeResult.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occured: {e.Error.Reason}");
                }
                catch (OperationCanceledException)
                {
                    _consumer.Close();
                }
            }

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}

