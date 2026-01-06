namespace model.game;

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
        hasX = Players.Any(p => p.Value == PlayerValue.X);
        hasO = Players.Any(p => p.Value == PlayerValue.O);
    }

    public Player AddPlayer(Player player)
    {
        CheckPlayers(player);

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
            var playerX = Players.FirstOrDefault(p => p.Value == PlayerValue.X);
            var playerO = Players.FirstOrDefault(p => p.Value == PlayerValue.O);
        }

        //Game(playerX, playerO);
        // Não sei o que você queria fazer aqui
    }
}
