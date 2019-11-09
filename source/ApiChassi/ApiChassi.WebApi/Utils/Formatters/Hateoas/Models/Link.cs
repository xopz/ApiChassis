using Newtonsoft.Json;

namespace AspNetCore.Hateoas.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class Link
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="href"></param>
        /// <param name="method"></param>
        public Link(string rel, string href, string method)
        {
            Rel = rel;
            Href = href;
            Method = method;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [JsonProperty(Order = 1)]
        public string Href { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [JsonProperty(Order = 0)]
        public string Rel { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [JsonProperty(Order = 2)]
        public string Method { get; private set; }
    }
}