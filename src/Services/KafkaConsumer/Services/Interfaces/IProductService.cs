using Shared.DTOs.Product;

namespace KafkaConsumer.Services;

public interface IProductService
{
    Task SaveSingleAsync(CreateProductDto product);
    Task SaveManyAsync(CreateProductDto[] products);
    Task SaveBulkAsync(CreateProductDto[] products);
    Task DeleteAsync(CreateProductDto product);
}