namespace ApiChassi.WebApi.Models._Shared
{
    using System.Collections.Generic;

    public class FindResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
    }
}
