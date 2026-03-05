using interfaces.requests;

namespace model.requests;

public record LeaveRequest : IRequest
{
    public string roomId {get; init;} = string.Empty;
}