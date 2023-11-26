namespace Demo.Api;

public sealed record ProductRequest
{
    public string? Name { get; init; }
    public int Quantity { get; init; }
}
