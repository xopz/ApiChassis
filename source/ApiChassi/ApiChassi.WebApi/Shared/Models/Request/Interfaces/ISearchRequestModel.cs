namespace ApiChassi.WebApi.Shared.Models.Request.Interfaces
{
    /// <summary>
    /// Defines the minimun required fields for request generic Search operations 
    /// </summary>
    public interface ISearchRequestModel
    {
        /// <summary>
        /// Defines how many records should be skipped
        /// </summary>
        uint Offset { get; set; }
        /// <summary>
        /// Limits how many records can return from the recordset
        /// </summary>
        ushort Limit { get; set; }
        /// <summary>
        /// Defines as comma separated values, the field names that should be used to sort the query
        /// </summary>
        string Order { get; set; }
        /// <summary>
        /// Defines as comma separated values, the field names that should be projected from the query
        /// </summary>
        string Fields { get; set; }
    }
}
