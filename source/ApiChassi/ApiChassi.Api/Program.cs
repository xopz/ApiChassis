using System.IO.Compression;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.AspNetCore.ResponseCompression;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add Open Telemetry
const string serviceName = "Example Api";
builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService(serviceName))
    .WithMetrics(builder => builder.AddAspNetCoreInstrumentation().AddConsoleExporter())
    .WithTracing(builder => builder.AddAspNetCoreInstrumentation().AddConsoleExporter());

// Add Healthcheck
builder.Services.AddHealthChecks();

// Add Compression
builder.Services
    .AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<GzipCompressionProvider>();
        options.Providers.Add<BrotliCompressionProvider>();
    })
    .Configure<GzipCompressionProviderOptions>(configuration => configuration.Level = CompressionLevel.Optimal)
    .Configure<BrotliCompressionProviderOptions>(configuration => configuration.Level = CompressionLevel.Optimal);

// Add Response Caching
builder.Services
    .AddResponseCaching()
    .AddVersionedApiExplorer();

// Add API versioning
builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.Conventions.Add(new VersionByNamespaceConvention());
        options.AssumeDefaultVersionWhenUnspecified = true;
    })
    .AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    });

builder.Services
    .AddControllers(options => options.RespectBrowserAcceptHeader = true)
    .AddXmlSerializerFormatters();

// Add OpenApi Docs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var versionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{versionDescription.GroupName}/swagger.json", versionDescription.GroupName.ToUpperInvariant());
    }
    // options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
