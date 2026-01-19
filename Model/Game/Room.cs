using System.Security.Cryptography;

namespace model.game;

public class Room
{
    public string id {get; init;} = string.Empty;

    public List<Player> Players {get; set;} = new ();

    public Room(Player player)
    {
        this.id = GenerateRoomId();
        this.Players.Add(player);
    }

    public static string GenerateRoomId(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVXZ0123456789";
        var bytes = new byte[length];

        RandomNumberGenerator.Fill(bytes);

        return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
    }
}