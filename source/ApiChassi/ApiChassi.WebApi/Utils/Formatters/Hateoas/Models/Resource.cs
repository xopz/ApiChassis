using System.Collections.Generic;
using Newtonsoft.Json;

namespace AspNetCore.Hateoas.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Resource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        protected Resource(object data)
        {
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data")]
        public virtual object Data { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Link"></typeparam>
        [JsonProperty("_links", Order = -2)]
        public virtual List<Link> Links { get; } = new List<Link>();
    }
}