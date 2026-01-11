using model.common;

namespace model.game;

public class Room : GenerateId
{
    public string id {get; init;} = string.Empty;

    public List<Player> Players {get; set;} = new ();

    public Room(Player player)
    {
        this.id = GenerateCode();
        this.Players.Add(player);
    }
}