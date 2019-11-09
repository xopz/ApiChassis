namespace ApiChassi.WebApi.V1.Models
{
    using ApiChassi.WebApi.Models.Request.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public class SampleFindRequestModel : IFindRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int _offset { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short _limit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string _order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string _fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
