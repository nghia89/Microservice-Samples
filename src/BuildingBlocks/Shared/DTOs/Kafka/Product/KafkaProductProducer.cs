namespace Shared.DTOs.Kafka;

public class KafkaProductProducer
{
    public long Id { get; set; }

    public string No { get; set; }

    public string Name { get; set; }

    public string Summary { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }
}

public class OrderProcessingRequest
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; }
}

public class OrderRequest
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; }
}