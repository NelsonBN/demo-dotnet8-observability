namespace Demo.Api;

public sealed record ProductResponse(int Id, string? Name, int Quantity)
{
    public static implicit operator ProductResponse(Product product)
        => new(
            product.Id,
            product.Name,
            product.Quantity);
}
