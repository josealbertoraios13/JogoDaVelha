using model.common;

namespace model.game;

public class Room
{
    public string id {get; init;} = string.Empty;

    public List<Player> Players {get; set;} = new ();

    public Room(Player player)
    {
        this.id = GenerateId.GenerateCode();
        this.Players.Add(player);
    }
}