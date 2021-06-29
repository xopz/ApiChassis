namespace ApiChassi.WebApi.Shared.Models.Request.Interfaces
{
    public interface ISearchRequestModel
    {
        uint Offset { get; set; }
        ushort Limit { get; set; }
        string Order { get; set; }
        string Fields { get; set; }
    }
}
