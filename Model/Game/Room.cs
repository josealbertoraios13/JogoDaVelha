namespace model.game;

public class Room
{
    public string id {get; init;} = string.Empty;

    public List<Player> Players {get; set;} = new ();

    public Room(Player player)
    {
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
    }
}