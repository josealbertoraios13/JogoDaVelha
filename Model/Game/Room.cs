namespace model.game;

public class Room : IdManager
{
    public string id {get; init;} = string.empty;
    public List<Player> Players {get;set;} = new ();

    public Room(Player player)
    {
        this.id = GenerateCode();
        AddPlayer(player);
    }

    public Player AddPlayer(Player player)
    {
        bool hasX = Players.Any(p => p.value == PlayerValue.X);
        bool hasO = Players.Any(p => p.value == PlayerValue.O);

        if(!hasX)
            player.value = PlayerValue.X;
        else if(!hasO)
            player.value = PlayerValue.O;
        else
            player.value = PlayerValue.Spectator;
        
        Players.Add(player);
        return player;
    }
}