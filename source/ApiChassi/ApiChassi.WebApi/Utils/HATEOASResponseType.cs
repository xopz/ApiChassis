namespace ApiChassi.WebApi.Utils
{
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class HATEOASResponseType : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Produces.Add("application/json+hateoas");
            operation.Produces.Add("application/xml+hateoas");
            operation.Produces.Add("text/json+hateoas");
            operation.Produces.Add("text/xml+hateoas");
        }
    }
}
