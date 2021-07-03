using ApiChassi.WebApi.Shared.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.IO.Compression;
using System.Net.Mime;
using System.Reflection;

namespace ApiChassi.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddHealthChecks();

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(configuration => configuration.Level = CompressionLevel.Optimal);
            services.Configure<GzipCompressionProviderOptions>(configuration => configuration.Level = CompressionLevel.Optimal);

            services
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

            services
                .AddControllers(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddXmlSerializerFormatters()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var result = new BadRequestObjectResult(context.ModelState);
                        result.ContentTypes.Add(MediaTypeNames.Application.Json);
                        result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                        return result;
                    };
                });

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.IncludeXmlComments(XmlCommentsFilePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            app.UseExceptionHandler("/error");

            var supportedCultures = new[] { "en-US", "pt-BR" };
            app.UseRequestLocalization(options =>
            {
                options
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures)
                    .SetDefaultCulture(supportedCultures[0]);
                options.FallBackToParentCultures = true;
                options.FallBackToParentUICultures = true;
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            app.UseCors();

            app.UseRouting();

            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var versionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{versionDescription.GroupName}/swagger.json", versionDescription.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = string.Empty;
            });
        }
    }
}
