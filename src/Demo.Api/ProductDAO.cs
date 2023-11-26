using System.Data;
using Dapper;

namespace Demo.Api;

public interface IProductDAO
{
    Task<IEnumerable<Product>> ListAsync();
    Task<Product?> GetAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

public sealed class ProductDAO(IDbConnection connection) : IProductDAO
{
    private readonly IDbConnection _connection = connection;

    public async Task<IEnumerable<Product>> ListAsync()
        => await _connection.QueryAsync<Product>(
            """
            SELECT
                Id,
                Name,
                Quantity
            FROM Product ;
            """);


    public async Task<Product?> GetAsync(int id)
        => await _connection.QuerySingleOrDefaultAsync<Product>(
            """
            SELECT
                Id,
                Name,
                Quantity
            FROM Product
            WHERE id = @id ;
            """,
            new { id });

    public async Task AddAsync(Product product)
        => product.Id = await _connection.ExecuteScalarAsync<int>(
            """
            INSERT INTO Product (Name , Quantity )
                         VALUES (@Name, @Quantity)
            SELECT CAST(SCOPE_IDENTITY() as int);
            """,
            product);

    public async Task UpdateAsync(Product product)
        => await _connection.ExecuteAsync(
        """
            UPDATE Product
            SET Name = @Name,
                Quantity = @Quantity
            WHERE id = @id ;
            """,
        new
        {
            product.Id,
            product.Name,
            product.Quantity
        });

    public async Task DeleteAsync(int id)
        => await _connection.ExecuteAsync(
            """
            DELETE FROM Product
            WHERE id = @id ;
            """,
            new { id });

    public async Task<bool> ExistsAsync(int id)
        => await _connection.ExecuteScalarAsync<bool>(
            """
            SELECT CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM Product
                    WHERE id = @id
                ) THEN CAST(1 AS BIT)
                ELSE CAST(0 AS BIT)
            END AS IdExists
            """,
            new { id });
}
