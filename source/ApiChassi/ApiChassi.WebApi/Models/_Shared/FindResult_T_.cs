namespace ApiChassi.WebApi.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FindResult<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
    }
}
