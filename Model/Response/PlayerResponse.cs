using interfaces.responses;

namespace model.responses;

public record PlayerResponse : IResponse
{
    public string id {get; init;} = string.Empty;
    public string name {get; init;} = string.Empty;
    public string avatar {get; init;} = string.Empty;
    public string type {get; init;} = string.Empty;
    public int wins {get; init;} = 0;
}