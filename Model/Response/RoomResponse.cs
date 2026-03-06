using interfaces.responses;

namespace model.responses;

public record RoomResponse : IResponse
{
    public string id {get; init;} = string.Empty;
    public List<PlayerResponse> players {get; set;} = new ();
}