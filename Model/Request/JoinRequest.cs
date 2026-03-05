using interfaces.requests;

namespace model.requests;

public record JoinRequest : IRequest
{
    public string roomId {get; init;} = string.Empty;
    public string name {get; init;} = string.Empty;
    public string avatar {get; init;} = string.Empty;
}