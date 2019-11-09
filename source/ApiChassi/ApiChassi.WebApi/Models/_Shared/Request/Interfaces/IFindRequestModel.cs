namespace ApiChassi.WebApi.Models.Request.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFindRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        int _offset { get; set; }

        /// <summary>
        /// 
        /// </summary>
        short _limit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string _order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string _fields { get; set; }
    }
}
