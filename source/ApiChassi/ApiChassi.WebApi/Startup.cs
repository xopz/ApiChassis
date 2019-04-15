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

    //public static class IMvcBuilderExtensions
    //{
    //    public static IMvcBuilder AddJsonHateoas(this IMvcBuilder builder) {
    //        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
    //        builder.AddMvcOptions(o => o.OutputFormatters.Add(new JsonHateoasOutputFormatter()));
    //        return builder;
    //    }
    //}

    //public class JsonHateoasOutputFormatter: TextOutputFormatter
    //{
    //    public JsonHateoasOutputFormatter()
    //    {
    //        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json+hateoas"));
    //        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json+hateoas"));

    //        SupportedEncodings.Add(Encoding.UTF8);
    //        SupportedEncodings.Add(Encoding.Unicode);
    //    }

    //    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    //    {
    //        var contextAccessor = GetService<IActionContextAccessor>(context);
    //        var urlHelperFactory = GetService<IUrlHelperFactory>(context);
    //        var actionDescriptorProvider = GetService<IActionDescriptorCollectionProvider>(context);
    //        var urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);

    //        var _object = context.Object;

    //        var _route = actionDescriptorProvider.ActionDescriptors.Items.FirstOrDefault(i => i.AttributeRouteInfo.Name == "update-record");
    //        var _method = _route.ActionConstraints.OfType<HttpMethodActionConstraint>().First().HttpMethods.First();
    //        //var _routeValues = default(object);
    //        var _options = new RouteValueDictionary((p) => new { id = p.Id });
    //        var _url = urlHelper.Link("update-record", _route.route).ToLowerInvariant();


    //        var _result = _object is IEnumerable ? 
    //            CreateHateoasCollection(_object as IEnumerable, actionDescriptorProvider) : 
    //            CreateHateoasObject(_object, actionDescriptorProvider);

    //        string _serializedData = JsonConvert.SerializeObject(_result);
    //        await context.HttpContext.Response.WriteAsync(_serializedData);
    //    }

    //    protected override bool CanWriteType(Type type)
    //    {
    //        return base.CanWriteType(type);
    //    }

    //    private T GetService<T>(OutputFormatterWriteContext context)
    //    {
    //        return (T)context.HttpContext.RequestServices.GetService(typeof(T));
    //    }

    //    private ExpandoObject CreateHateoasObject(object o, IActionDescriptorCollectionProvider actionDescriptorCollection)
    //    {
    //        dynamic _r = new ExpandoObject();
    //        foreach (var property in o.GetType().GetProperties())
    //        {
    //            if (property.CanRead)
    //            {
    //                (_r as IDictionary<string, object>).Add(property.Name, property.GetValue(o));
    //            }
    //        }
    //        _r._links = new List<Link> {
    //                Link.Self(o),
    //                Link.Update(o),
    //                Link.Delete(o)
    //        };
    //        return _r;
    //    }

    //    private ExpandoObject CreateHateoasCollection(IEnumerable o, IActionDescriptorCollectionProvider actionDescriptorCollection)
    //    {
    //        dynamic _object = new ExpandoObject();
    //        _object.data = o.Select(s => CreateHateoasObject(s, actionDescriptorCollection));
    //        _object._links = new List<Link> {
    //            Link.FirstPage(10, 0, 1),
    //            new Link { rel = "previous-page"},
    //            new Link { rel = "next-page"},
    //            Link.LastPage(10, 0, 1),
    //        };
    //        return _object;
    //    }
    //}

    //public class Link
    //{
    //    public string href { get; set; }
    //    public string rel { get; set; }
    //    public string method { get; set; }

    //    public static Link Self<T>(T obj)
    //    {
    //        return new Link
    //        {
    //            rel = "self",
    //            method = "GET",
    //            href = $"http://xopz.net/{1}"
    //        };
    //    }

    //    public static Link Update<T>(T obj)
    //    {
    //        return new Link
    //        {
    //            rel = "update-record",
    //            method = "PUT",
    //            href = $"http://xopz.net/{1}"
    //        };
    //    }

    //    public static Link Delete<T>(T obj)
    //    {
    //        return new Link
    //        {
    //            rel = "delete-record",
    //            method = "DELETE",
    //            href = $"http://xopz.net/{1}"
    //        };
    //    }

    //    public static Link FirstPage(short currentLimit, int currentOffset, int totalRecords)
    //    {
    //        return new Link
    //        {
    //            rel = "first-page",
    //            method = "GET",
    //            href = $"http://xopz.net/?_limit={currentLimit}"
    //        };
    //    }

    //    //public static Link PreviousPage(short currentLimit, int currentOffset, int totalRecords)
    //    //{

    //    //}

    //    //public static Link NextPage(short currentLimit, int currentOffset, int totalRecords)
    //    //{

    //    //}

    //    public static Link LastPage(short currentLimit, int currentOffset, int totalRecords)
    //    {
    //        return new Link
    //        {
    //            rel = "last-page",
    //            method = "GET",
    //            href = $"http://xopz.net/?_limit={currentLimit}&_offset={Math.Ceiling((double)(totalRecords / currentLimit))}"
    //        };
    //    }
    //}

    //public class HateoasObject : Expando
    //{
    //    public IEnumerable<Link> _links { get; set; }

    //    public HateoasObject(object data, string baseUrl) : base(data)
    //    {
    //        //this.SetProperty(this, "_links", new[]
    //        //{
    //        //    new Link { rel = "update", method = "PUT", href = $"{baseUrl}/123" },
    //        //    new Link { rel = "delete", method = "DELETE", href = $"{baseUrl}/123" },
    //        //    new Link { rel = "self", method = "GET", href = $"{baseUrl}/123" }

    //        //}.AsEnumerable());
    //    }
    //}

    //public class HateoasCollection
    //{
    //    public List<HateoasObject> Data { get; set; }

    //    public IEnumerable<Link> _links { get; set; }

    //    public HateoasCollection(IEnumerable data, string baseUrl)
    //    {
    //        Data = new List<HateoasObject>();
    //        foreach (var obj in data)
    //        {
    //            dynamic dobj = new HateoasObject(obj, baseUrl);
    //            dobj._links = new[]
    //            {
    //                new Link { rel = "update", method = "PUT", href = $"{baseUrl}/123" },
    //                new Link { rel = "delete", method = "DELETE", href = $"{baseUrl}/123" },
    //                new Link { rel = "self", method = "GET", href = $"{baseUrl}/123" }

    //            }.AsEnumerable();

    //            Data.Add(dobj);
    //        }
    //    }
    //}
}
