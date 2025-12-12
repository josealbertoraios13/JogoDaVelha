using System;

namespace model.game;

public class Room
{
    public string id {get; init;} = string.Empty;

    public List<Player> Players {get; set;} = new ();

    public Room(string id, Player player)
    {
        this.id = id;
        this.Players.Add(player);
    }
}