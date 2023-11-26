namespace Demo.Api;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/products", async (ILogger<Program> logger, IProductDAO dao) =>
        {
            var products = (await dao
                .ListAsync())
                .Select(product => (ProductResponse)product);

            logger.LogInformation("[LIST] Found {Count} products.", products.Count());

            return Results.Ok(products);
        });



        endpoints.MapGet("/products/{id}", async (ILogger<Program> logger, IProductDAO dao, int id) =>
        {
            var product = await dao.GetAsync(id);
            if(product is null)
            {
                return Results.NotFound();
            }

            logger.LogInformation("[GET] Found product {Id}.", product.Id);

            return Results.Ok((ProductResponse)product);
        }).WithName("GetProduct");



        endpoints.MapPost("/products", async (ILogger<Program> logger, IProductDAO dao, ProductRequest request) =>
        {
            if(string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest("Name is required.");
            }

            var product = new Product
            {
                Name = request.Name,
                Quantity = request.Quantity
            };

            await dao.AddAsync(product);

            logger.LogInformation("[CREATED] Product {Id}.", product.Id);

            return Results.CreatedAtRoute(
                "GetProduct",
                new { id = product.Id },
                (ProductResponse)product);
        });



        endpoints.MapPut("/products/{id}", async (ILogger<Program> logger, IProductDAO dao, int id, ProductRequest request) =>
        {
            if(string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest("Name is required.");
            }

            var product = await dao.GetAsync(id);
            if(product is null)
            {
                return Results.NotFound();
            }

            product.Name = request.Name;
            product.Quantity = request.Quantity;

            await dao.UpdateAsync(product);

            logger.LogInformation("[UPDATED] Product {Id}.", product.Id);

            return Results.NoContent();
        });



        endpoints.MapDelete("/products/{id}", async (ILogger<Program> logger, IProductDAO dao, int id) =>
        {
            if(!await dao.ExistsAsync(id))
            {
                return Results.NotFound();
            }

            await dao.DeleteAsync(id);

            logger.LogInformation("[DELETED] Product {Id}.", id);

            return Results.NoContent();
        });
    }
}
