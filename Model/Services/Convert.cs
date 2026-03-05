using model.game;
using model.responses;

public class Convert
{
    public static List<PlayerResponse> PlayerListToResponseList(List<Player> players)
    {
        List<PlayerResponse> playerResponses = new();

        foreach (var p in players)
        {
            var convertedPlayer = PlayerToResponse(p);
            playerResponses.Add(convertedPlayer);
        }

        return playerResponses;
    }

    public static PlayerResponse PlayerToResponse(Player player)
    {
        return new PlayerResponse()
        {
            id = player.id,
            name = player.name,
            avatar = player.avatar,
            type = player.type,
            wins = player.wins
        };
    }

    public static RoomResponse RoomToResponse(Room room)
    {
        return new RoomResponse() 
        {
            id = room.id,
            players = Convert.PlayerListToResponseList(room.players)
        }; 
    }
}