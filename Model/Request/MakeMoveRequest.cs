using interfaces.requests;
using model.game;

namespace model.requests;

public record MakeMoveRequest : IRequest
{
    public Block Block {get; init;} 
    public string roomId {get; init;} = string.Empty;
}