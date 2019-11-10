namespace ApiChassi.WebApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.ResponseCompression;
    using System.IO.Compression;
    using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using System.IO;
    using System;
    using System.Reflection;
    using Swashbuckle.AspNetCore.Swagger;
    using ApiChassi.WebApi.Utils.Extensions;
    using Newtonsoft.Json;
    using ApiChassi.WebApi.V1.Models;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        static string XmlCommentsFilePath
        {
            get
            {
                return Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Add CORS
            services.AddCors();

            //Add HealthCheck endpoint
            services.AddHealthChecks();

            //Add HTTP Compression
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(o => { o.Level = CompressionLevel.Fastest; });
            services.Configure<GzipCompressionProviderOptions>(o => { o.Level = CompressionLevel.Fastest; });

            //Add API Versioning and Version Explorer
            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
                // automatically applies an api version based on the name of the defining controller's namespace
                options.Conventions.Add(new VersionByNamespaceConvention());
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";
                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });


            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = true;
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddHateoas(options =>
                {
                    options
                        .AddLink<SampleModel>("update-record", s => new { id = s.Id })
                        .AddLink<SampleModel>("delete-record", s => new { id = s.Id })
                        .AddLink<SampleModel>("get-record", s => new { id = s.Id })
                        .AddLink<IEnumerable<SampleModel>>("create-record");
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.None;
                })
                .AddXmlSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            //Add Swagger documentation GUI
            services.AddSwaggerGen(options =>
            {
                // resolve the IApiVersionDescriptionProvider service
                // note: that we have to build a temporary service provider here because one has not been created yet
                using (var serviceProvider = services.BuildServiceProvider())
                {
                    var _provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                    // add a swagger document for each discovered API version
                    // note: you might choose to skip or document deprecated API versions differently
                    foreach (var _description in _provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(_description.GroupName, CreateInfoForApiVersion(_description));
                    }
                }
                options.IncludeXmlComments(XmlCommentsFilePath);
                //options.OperationFilter<HATEOASResponseType>();
            });

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors();
            //app.UseJsonExceptionHandler();
            app.UseHealthChecks();
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var _description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{_description.GroupName}/swagger.json",
                        _description.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = string.Empty;
            });
            app.UseMvc();
        }

        static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info
            {
                Title = "Sample API",
                Version = description.ApiVersion.ToString(),
                Description = "A sample application with Swagger, Swashbuckle, and API versioning.",
                Contact = new Contact { Name = "Murilo Beltrame", Email = "murilobeltrame@somewhere.com" },
                TermsOfService = "Freeware",
                License = new License { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
