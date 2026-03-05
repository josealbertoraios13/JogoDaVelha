using interfaces.responses;

namespace model.responses;

public record MessageResponse : IResponse
{
    public string playerID {get; init;} = string.Empty;
    public string message {get; init;} = string.Empty;
    public DateTime createdAt {get; init;}
}