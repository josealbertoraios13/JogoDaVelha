using model.game;
using model.responses;

namespace interfaces.convert;

public interface IConvert
{
    public PlayerResponse PlayerToResponse(Player player);
    public RoomResponse RoomToResponse(Room room);
    public List<PlayerResponse> PlayerListToResponseList(List<Player> players);
}