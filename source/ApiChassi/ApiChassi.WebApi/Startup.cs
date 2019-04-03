using System.Collections;
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
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public class Startup
    {
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

        public IConfiguration Configuration { get; }

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
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.Conventions.Add(new VersionByNamespaceConvention());
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services
                .AddMvc(options => {
                    options.EnableEndpointRouting = true;
                    options.RespectBrowserAcceptHeader = true;
                    options.OutputFormatters.Insert(0, new HateoasOutputFormatter());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.None;
                })
                .AddXmlSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddSwaggerGen(options =>
            {
                using (var serviceProvider = services.BuildServiceProvider())
                {
                    var _provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var _description in _provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(_description.GroupName, CreateInfoForApiVersion(_description));
                    }
                }
                options.IncludeXmlComments(XmlCommentsFilePath);
                //options.OperationFilter<HATEOASResponseType>();
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
                Contact = new Contact { Name = "Bill Mei", Email = "murilobeltrame@somewhere.com" },
                TermsOfService = "Shareware",
                License = new License { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }

    public class HateoasOutputFormatter: TextOutputFormatter
    {
        public HateoasOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json+hateoas"));
            //SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/xml+hateoas"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json+hateoas"));
            //SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/xml+hateoas"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var _object = context.Object;
            object _hateoasObject = null;
            if (_object is IEnumerable)
            {
                _hateoasObject = new HateoasCollection(_object as IEnumerable, "www.google.com");
            }
            else
            {
                _hateoasObject = new HateoasObject(_object, "www.google.com");
            }
            var _serializedData = JsonConvert.SerializeObject(_hateoasObject);
            await context.HttpContext.Response.WriteAsync(_serializedData);
        }

        protected override bool CanWriteType(Type type)
        {
            return base.CanWriteType(type);
        }
    }

    public class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }

    public class HateoasObject
    {
        public object Data { get; set; }

        public IEnumerable<Link> _links { get; set; }

        public HateoasObject(object data, string baseUrl)
        {
            Data = data;
            _links = new[]
            {
                new Link { rel = "update", method = "PUT", href = $"{baseUrl}/123" },
                new Link { rel = "delete", method = "DELETE", href = $"{baseUrl}/123" },
                new Link { rel = "self", method = "GET", href = $"{baseUrl}/123" }

            }.AsEnumerable();
        }
    }

    public class HateoasCollection
    {
        public List<HateoasObject> Data { get; set; }

        public HateoasCollection(IEnumerable data, string baseUrl)
        {
            Data = new List<HateoasObject>();
            foreach (var obj in data)
            {
                Data.Add(new HateoasObject(obj, baseUrl));
            }
        }
    }
}
