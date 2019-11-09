using Newtonsoft.Json;

namespace AspNetCore.Hateoas.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ListItemResource : Resource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ListItemResource(object data) : base(data)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("items")]
        public override object Data => base.Data;
    }
}