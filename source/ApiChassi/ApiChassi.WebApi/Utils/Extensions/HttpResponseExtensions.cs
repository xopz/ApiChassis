namespace ApiChassi.WebApi.Utils.Extensions
{
    using System.Text;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.WebUtilities;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public static class HttpResponseExtensions
    {
        static readonly JsonSerializer Serializer = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="obj"></param>
        /// <param name="contentType"></param>
        /// <typeparam name="T"></typeparam>
        public static void WriteJson<T>(this HttpResponse response, T obj, string contentType = null)
        {
            response.ContentType = contentType ?? "application/json";
            using (var writer = new HttpResponseStreamWriter(response.Body, Encoding.UTF8))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.CloseOutput = false;
                    jsonWriter.AutoCompleteOnClose = false;

                    Serializer.Serialize(jsonWriter, obj);
                }
            }
        }
    }
}
