namespace ApiChassi.WebApi.Utils.Extensions
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Server.Kestrel.Core;

    /// <summary>
    /// 
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        {
            return app.UseHealthChecks("/health");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseJsonExceptionHandler(this IApplicationBuilder app)
        {
            app.Run(async context => await Task.Run(() =>
             {
                 var _errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                 var _exception = _errorFeature.Error;

                 var _problemDetails = new ProblemDetails
                 {
                     Instance = $"urn:minhati:error:{Guid.NewGuid()}"
                 };

                 if (_exception is BadHttpRequestException badHttpRequestException)
                 {
                     _problemDetails.Title = "Invalid request";
                     _problemDetails.Status = (int)typeof(BadHttpRequestException).GetProperty("StatusCode", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(badHttpRequestException);
                     _problemDetails.Detail = badHttpRequestException.Message;
                 }
                 else
                 {
                     _problemDetails.Title = "An unexpected error occurred!";
                     _problemDetails.Status = 500;
                     _problemDetails.Detail = _exception.StackTrace;
                 }

                 context.Response.StatusCode = _problemDetails.Status.Value;
                 context.Response.WriteJson(_problemDetails, "application/problem+json");
             }));

            return app;
        }
    }
}
