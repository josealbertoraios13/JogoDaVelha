namespace model.game;

public class Room : ModelGeralStructure
{
    public List<Player> Players {get; set;} = new ();
    public Player? playerX {get; set;}
    public Player? playerO {get; set;}

    public Room(Player player)
    {
<<<<<<< HEAD
        this.id = GetId();
        this.Players.Add(player);
    }

    public static string GetId()
    {
        var random = new Random();
        var id = string.Empty;

        for(int i = 0; i < 6; i++)
        {
            id += random.Next(0, 9);
        }

        return id;
=======
        GenerateId();
        this.Players.Add(player);
    }

    public void PlayersType()
    {
        if (Players.Count >= 2)
        {
            playerX = Players.FirstOrDefault(p => p.type == "X");
            playerO = Players.FirstOrDefault(p => p.type == "O");
        }
>>>>>>> 026c084 (Validações e gerar id)
    }
}