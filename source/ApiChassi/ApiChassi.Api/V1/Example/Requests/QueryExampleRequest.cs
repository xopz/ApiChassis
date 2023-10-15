namespace ApiChassi.Api.V1.Example.Requests;

public class QueryExampleRequest
{
    public string Description { get; set; } = string.Empty;
    public uint Skip { get; set; }
    public ushort Take { get; set; }
    public string Sort { get; set; } = string.Empty;
}
