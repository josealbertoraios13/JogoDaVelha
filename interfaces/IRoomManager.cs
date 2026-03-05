using interfaces.convert;
using interfaces.responses;
using model.game;

namespace interfaces.roomManager;

public interface IRoomManager
{
    Room CreateRoom(string connectionId, string name, string avatar);
    Room JoinRoom(string roomId, string connectionId, string name, string avatar, out Player newPlayer);
    (Room room, Player player) LeaveRoom(string roomId, string connectionId);
    IResponse ExecuteMove(string roomId, string connectionId, int x, int y, IConvert _convert, out Room selectedRoom);
    Room GetRoom(string roomId);
}