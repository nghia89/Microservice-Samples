using Nest;
using Shared.DTOs.Kafka;

namespace KafkaConsumer.Extensions;

public static class ElasticSearchExtensions
{
    public static void AddElasticsearch(
        this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["ElasticConfiguration:Uri"];
        var defaultIndex = configuration["ElasticConfiguration:Index"];
        var username = configuration.GetValue<string>("ElasticConfiguration:Username");
        var password = configuration.GetValue<string>("ElasticConfiguration:Password");

        var settings = new ConnectionSettings(new Uri(url)).BasicAuthentication(username, password)
            .PrettyJson()
            .DefaultIndex(defaultIndex);

        AddDefaultMappings(settings);

        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);

        CreateIndex(client, defaultIndex);
    }

    private static void AddDefaultMappings(ConnectionSettings settings)
    {
        settings
            .DefaultMappingFor<KafkaProductProducer>(m => m
                .Ignore(p => p.Price)
            );
    }

    private static void CreateIndex(IElasticClient client, string indexName)
    {
        var createIndexResponse = client.Indices.Create(indexName,
            index => index.Map<KafkaProductProducer>(x => x.AutoMap())
        );
    }
}