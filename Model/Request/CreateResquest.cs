using interfaces.requests;

namespace model.requests;

public record CreateResquest : IRequest
{
    public string name {get; init;} = string.Empty;
    public string avatar {get; init;} = string.Empty;
}