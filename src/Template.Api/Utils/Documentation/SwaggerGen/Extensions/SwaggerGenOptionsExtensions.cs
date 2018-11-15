using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Template.Api.Utils.Swagger.SwaggerGen.Extensions
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void SwaggerDocByApiVersion(this SwaggerGenOptions options, ApiVersionDescription description)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var _version = description.ApiVersion.ToString();
            var _info = new Info
            {
                Title = "", //TODO: Get from ...
                Version = _version,
                Description = "" //TODO: Get from ...
            };
            if (description.IsDeprecated) { }
            return _info;
        }
    }
}