using interfaces.responses;

namespace model.responses;

public record PlayerResponse : IResponse
{
    public string id {get; set;} = string.Empty;
    public string name {get; set;} = string.Empty;
    public string avatar {get; set;} = string.Empty;
    public string type {get; set;} = string.Empty;
    public int wins {get; set;} = 0;
}