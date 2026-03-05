using interfaces.requests;

namespace model.requests;

public record MessageRequest : IRequest
{
    public string roomId {get; init;} = string.Empty;
    public string message {get; init;} = string.Empty;
    public DateTime createdAt {get; init;}
}