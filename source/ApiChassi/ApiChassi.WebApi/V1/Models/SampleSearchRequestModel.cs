using ApiChassi.WebApi.Shared.Models.Request.Interfaces;

namespace ApiChassi.WebApi.V1.Models
{
    public class SampleSearchRequestModel : ISearchRequestModel
    {
        public uint Offset { get; set; }
        public ushort Limit { get; set; }
        public string Order { get; set; }
        public string Fields { get; set; }
        public string Description { get; set; }
    }
}
