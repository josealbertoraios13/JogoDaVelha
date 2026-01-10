namespace model.game;

using model.game.enums;

public class Room : IdManager
{
    public string id {get; init;} = string.Empty;
    public List<Player> Players {get;set;} = new ();

    private bool hasX;
    private bool hasO;

    public Room(Player player)
    {
        this.id = GenerateCode();
        AddPlayer(player);
    }

    public void CheckPlayers(Player player)
    {
        bool hasX = Players.Any(p => p.value = PlayerValue.X);
        bool hasO = Players.Any(p => p.value = PlayerValue.O);

    }

    public Player AddPlayer(Player player, bool hasX, bool hasO)
    {
        if(!hasX)
            player.Value = PlayerValue.X;
        else if(!hasO)
            player.Value = PlayerValue.O;
        else
            player.Value = PlayerValue.Spectator;
        
        Players.Add(player);
        return player;
    }

    public void SetGame(Player player)
    {
        CheckPlayers(player);
        
        if (hasX && hasO)
        {
            Player playerX = Players.FirstOrDefault(p => p.value = PlayerValue.X);
            Player playerO = Players.FirstOrDefault(p => p.value = PlayerValue.O);
        }

        //Game(playerX, playerO);
    }
}