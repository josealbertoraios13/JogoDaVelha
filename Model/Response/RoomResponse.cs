using interfaces.responses;
using model.game;

namespace model.responses;

public record RoomResponse : IResponse
{
    public string id {get; init;} = string.Empty;
    public List<Player> players {get; set;} = new ();
}