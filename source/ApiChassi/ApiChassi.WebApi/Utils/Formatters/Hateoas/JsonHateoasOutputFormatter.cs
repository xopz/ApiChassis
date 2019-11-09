using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Hateoas.Infrastructure;
using AspNetCore.Hateoas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AspNetCore.Hateoas.Formatters
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonHateoasOutputFormatter : JsonOutputFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ApplicationJsonHateoas = "application/json+hateoas";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializerSettings"></param>
        /// <param name="charPool"></param>
        public JsonHateoasOutputFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool)
            : base(serializerSettings, charPool)
        {
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add(ApplicationJsonHateoas);

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        /// <inheritdoc />
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
            Encoding selectedEncoding)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (selectedEncoding == null)
                throw new ArgumentNullException(nameof(selectedEncoding));

            using (var writer = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            {
                WriteObject(writer, GetObjectToFormat(context));
                await writer.FlushAsync();
            }
        }

        private T GetService<T>(OutputFormatterWriteContext context)
        {
            return (T)context.HttpContext.RequestServices.GetService(typeof(T));
        }

        private object GetObjectToFormat(OutputFormatterWriteContext context)
        {
            switch (context.Object)
            {
                case SerializableError error:
                    return error;
                case Resource existingResource:
                    return existingResource;
                default:
                    return CreateResourceFactory(context).CreateResource(context, context.Object);
            }
        }

        private ResourceFactory CreateResourceFactory(OutputFormatterWriteContext context)
        {
            var options = GetService<IOptions<HateoasOptions>>(context).Value;
            var actionDescriptorProvider = GetService<IActionDescriptorCollectionProvider>(context);
            var urlHelper = GetService<IUrlHelperFactory>(context)
                .GetUrlHelper(GetService<IActionContextAccessor>(context).ActionContext);

            return new ResourceFactory(options, actionDescriptorProvider, urlHelper);
        }

        private class ResourceFactory
        {
            private readonly IActionDescriptorCollectionProvider _actionDescriptorProvider;
            private readonly HateoasOptions _options;
            private readonly IUrlHelper _urlHelper;

            public ResourceFactory(HateoasOptions options,
                IActionDescriptorCollectionProvider actionDescriptorProvider,
                IUrlHelper urlHelper)
            {
                _options = options;
                _actionDescriptorProvider = actionDescriptorProvider;
                _urlHelper = urlHelper;
            }

            public Resource CreateResource(OutputFormatterWriteContext context, object result)
            {
                var (isSequence, elementType) = IsISequence(typeof(IEnumerable<>), context.ObjectType);

                if (!isSequence)
                    return CreateObjectResource(context.ObjectType, context.Object);

                var resourceList = ((IEnumerable<object>)result)
                    .Select(r => CreateObjectResource(elementType, r))
                    .ToList();
                return CreateListResource(context.ObjectType, resourceList);
            }

            private Resource CreateObjectResource(Type type, object value)
            {
                var resource = new ObjectResource(value);

                return AppendLinksToResource(type, resource, false);
            }

            private Resource CreateListResource(Type type, object value)
            {
                var resource = new ListItemResource(value);

                return AppendLinksToResource(type, resource, true);
            }

            private Link CreateLink(ILinksRequirement option, string method, object routeValues)
            {
                var url = _urlHelper.Link(option.Name, routeValues).ToLower();
                var link = new Link(option.Name, url, method);
                return link;
            }

            private Resource AppendLinksToResource(Type type, Resource resource, bool isEnumerable)
            {
                var resourceOptions = _options.Requirements
                    .Where(r => r.ResourceType == type)
                    .Where(r => isEnumerable || r.IsLinkAllowed(resource.Data));

                foreach (var option in resourceOptions)
                {
                    var route = _actionDescriptorProvider
                            .ActionDescriptors
                            .Items
                            .FirstOrDefault(i => i.AttributeRouteInfo.Name == option.Name)
                        ?? throw new ArgumentException($"Route with name {option.Name} cannot be found");

                    var method = route
                        .ActionConstraints
                        .OfType<HttpMethodActionConstraint>()
                        .First()
                        .HttpMethods
                        .First();

                    var routeValues = isEnumerable
                        ? default(object)
                        : option.GetRouteValues(resource.Data);

                    var link = CreateLink(option, method, routeValues);
                    resource.Links.Add(link);
                }

                return resource;
            }

            private static (bool, Type) IsISequence(Type sequenceInterface, Type source)
            {
                var type = source.GetInterface(sequenceInterface.Name, false);
                if (type == null && source.IsGenericType && source.GetGenericTypeDefinition() == sequenceInterface)
                    type = source;
                if (type == null) return (false, null);

                var element = type.GetGenericArguments()[0];
                return (!element.IsGenericParameter, element);
            }
        }
    }
}