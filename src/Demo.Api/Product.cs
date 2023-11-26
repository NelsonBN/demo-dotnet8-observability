namespace Demo.Api;

public sealed class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Quantity { get; set; }
}
