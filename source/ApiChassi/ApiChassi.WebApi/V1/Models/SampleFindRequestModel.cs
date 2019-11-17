namespace ApiChassi.WebApi.V1.Models
{
    using ApiChassi.WebApi.Models.Request.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public class SampleFindRequestModel : IFindRequestModel
    {
        /// <summary>
        /// Records offset used as reference starting point for limit.
        /// </summary>
        /// <example>0</example>
        public int _offset { get; set; }

        /// <summary>
        /// Amount of records to be fetched from query
        /// </summary>
        /// <example>10</example>
        public short _limit { get; set; }

        /// <summary>
        /// Specify a field name form wich the resultset will be sorted
        /// </summary>
        /// <example>Description</example>
        public string _order { get; set; }

        /// <summary>
        /// Specify the field names to be regrieved from resultset. If no value specified, all fields in the model will be returned
        /// </summary>
        /// <example>id,description</example>
        public string _fields { get; set; }

        /// <summary>
        /// Search for records that have the same description. No filter will be applied if no value specified
        /// </summary>
        /// <example>Test</example>
        public string Description { get; set; }
    }
}
