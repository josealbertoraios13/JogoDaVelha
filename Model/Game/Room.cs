using System;

namespace model.game;

public class Room : ModelStructure
{
    public List<Player> Players {get; set;} = new ();
    public Player? playerX {get; set;}
    public Player? playerO {get; set;}

    public Room(string id, Player player)
    {
        GenerateId();
        this.Players.Add(player);
    }

    public void PlayersType()
    {
        if (Players.Count >= 2)
        {
            playerX = Players.FirstOrDefault(p => p.type == "X");
            playerO = Players.FirstOrDefault(p => p.type == "O"); //!
        }
    }
}