using System.Collections.Generic;
using System.Linq;

namespace ApiChassi.WebApi.Shared.Models
{
    /// <summary>
    /// A structure to hold the query result as an intermediate for generic Search operations
    /// </summary>
    /// <typeparam name="T">Type of the items in the recordset</typeparam>
    public record SearchResult<T>
    {
        /// <summary>
        /// The paged recordset returned from the query
        /// </summary>
        public IEnumerable<T> Data { get; init; }

        /// <summary>
        /// Total record count from the query
        /// </summary>
        public uint TotalCount { get; init; }

        /// <summary>
        /// Initialize this structure considering the record count as the length of the recordset
        /// </summary>
        /// <param name="data">The recordset</param>
        public SearchResult(IEnumerable<T> data) => (Data, TotalCount) = (data, (uint)data.Count());

        /// <summary>
        /// Initialize this structure setting the recordset and the total record count individually
        /// </summary>
        /// <param name="data">The recordset</param>
        /// <param name="totalCount">The record cound</param>
        public SearchResult(IEnumerable<T> data, uint totalCount) => (Data, TotalCount) = (data, totalCount);
    }
}
