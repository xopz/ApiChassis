namespace AspNetCore.Hateoas.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectResource : Resource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ObjectResource(object data) : base(data) { }
    }
}