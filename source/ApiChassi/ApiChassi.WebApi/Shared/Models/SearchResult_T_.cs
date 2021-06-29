using System.Collections.Generic;
using System.Linq;

namespace ApiChassi.WebApi.Shared.Models
{
    public record SearchResult<T>
    {
        public IEnumerable<T> Data { get; init; }
        public uint TotalCount { get; init; }

        public SearchResult(IEnumerable<T> data) => (Data, TotalCount) = (data, (uint)data.Count());

        public SearchResult(IEnumerable<T> data, uint totalCount) => (Data, TotalCount) = (data, totalCount);
    }
}
