using Nest;
using Shared.DTOs.Product;

namespace KafkaConsumer.Services;

public class ProductService : IProductService
{
    private readonly IElasticClient _elasticClient;
    public ProductService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }
    public async Task SaveSingleAsync(CreateProductDto product)
    {
        if (!String.IsNullOrEmpty(product.No) && product.No != "string")
        {
            await _elasticClient.UpdateAsync<CreateProductDto>(product, u => u.Doc(product));
        }
        else
        {
            await _elasticClient.IndexDocumentAsync(product);
        }
    }

    public async Task SaveManyAsync(CreateProductDto[] products)
    {
        var result = await _elasticClient.IndexManyAsync(products);
        if (result.Errors)
        {
            // the response can be inspected for errors
            foreach (var itemWithError in result.ItemsWithErrors)
            {
                Console.WriteLine("Failed to index document {0}: {1}",
                    itemWithError.Id, itemWithError.Error);
            }
        }
    }

    public async Task SaveBulkAsync(CreateProductDto[] products)
    {
        var result = await _elasticClient.BulkAsync(b => b.Index("products").IndexMany(products));
        if (result.Errors)
        {
            // the response can be inspected for errors
            foreach (var itemWithError in result.ItemsWithErrors)
            {
                Console.WriteLine("Failed to index document {0}: {1}",
                    itemWithError.Id, itemWithError.Error);
            }
        }
    }

    public async Task DeleteAsync(CreateProductDto product)
    {
        await _elasticClient.DeleteAsync<CreateProductDto>(product);
    }
}