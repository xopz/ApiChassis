using System.IO.Compression;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Template.Api.Utils.Swagger.SwaggerGen;
using Template.Api.Utils.Swagger.SwaggerGen.Extensions;

namespace Template.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // This configuration enables HTTP Compression with Gzip
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services
                .AddMvc()
                .AddJsonOptions(options => 
                {
                    // This configuration sets optimization for JSON serializing 
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.None;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // This configuration adds support for API versioning
            services
                .AddMvcCore()
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
            services.AddApiVersioning(options => options.ReportApiVersions = true);

            // This configuration enables Swagger generation
            services.AddSwaggerGen(options =>
            {
                var _provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var _description in _provider.ApiVersionDescriptions)
                {
                    options.SwaggerDocByApiVersion(_description);
                }
                options.OperationFilter<SwaggerDefaultValues>();
                var _xmlDocFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(_xmlDocFileName);

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;
                foreach (var _description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{_description.GroupName}/swagger.json", _description.GroupName.ToUpperInvariant());
                }
            });
            app.UseResponseCompression();
            app.UseMvc();
        }
    }
}
