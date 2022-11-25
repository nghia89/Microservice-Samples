namespace Basket.Api.Entities;

public class Cart
{
    public string UserName { get; set; }
    public Cart() { }
    public Cart(string username)
    {
        UserName = username;
    }
    public List<CartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(a => a.Quantity * a.ItemPrice);
    public DateTimeOffset LastModifiedDate { get; set; } = DateTimeOffset.UtcNow;

}