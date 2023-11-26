using System.Data;
using Demo.Api;
using Microsoft.Data.SqlClient;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddScoped<IProductDAO, ProductDAO>();

builder.Services
    .AddScoped<IDbConnection>(sp =>
        new SqlConnection(sp.GetRequiredService<IConfiguration>().GetConnectionString("Default")));

builder.Services
    .AddOpenTelemetry()
    .WithMetrics(builder =>
    {
        builder.AddPrometheusExporter();

        builder.AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel");

        builder.AddView("http.server.request.duration",
            new ExplicitBucketHistogramConfiguration
            {
                Boundaries = [0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10]
            });
    });

var app = builder.Build();

app.MapProductsEndpoints();
app.MapPrometheusScrapingEndpoint();

app.Run();
