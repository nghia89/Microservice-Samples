using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using Shared.DTOs.Kafka;
using Shared.DTOs.Product;

namespace ApacheKafkaProducerDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly string
        bootstrapServers = "localhost:9092";
        private readonly string topic = "product";

        [HttpPost]
        public async Task<IActionResult>
        Post([FromBody] CreateProductDto orderRequest)
        {
            string message = JsonSerializer.Serialize(orderRequest);
            for (int i = 0; i < 200; i++)
            {
                await SendOrderRequest(topic, message);
            }

            //await SendOrderRequest("product01", message);
            return Ok(await SendOrderRequest(topic, message));
        }
        private async Task<bool> SendOrderRequest(string topic, string message)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var result = await producer.ProduceAsync(topic, new Message<Null, string>
                    {
                        Value = message
                    });

                    Debug.WriteLine($"Delivery Timestamp:{result.Timestamp.UtcDateTime} _ Offset: {result.Offset}");
                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}
