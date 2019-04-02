namespace ApiChassi.WebApi.V1.Models
{
    using ApiChassi.WebApi.Models.Request.Interfaces;

    public class SampleFindRequestModel : IFindRequestModel
    {
        public int _offset { get; set; }
        public short _limit { get; set; }
        public string _order { get; set; }
        public string _fields { get; set; }
        public string Description { get; set; }
    }
}
