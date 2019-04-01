namespace ApiChassi.WebApi.Models.Request.Interfaces
{
    public interface IFindRequestModel
    {
        int _offset { get; set; }
        short _limit { get; set; }
        string _order { get; set; }
        string _fields { get; set; }
    }
}
