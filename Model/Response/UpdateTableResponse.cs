using interfaces.responses;

namespace model.responses;

public record UpdateTableResponse : IResponse
{
    public string currentTurn {get; init;} = string.Empty;
    public string?[][] ?table {get; init;}
}