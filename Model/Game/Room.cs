using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace model.game;

public class Room
{
    public string id {get; init;} = string.Empty;

    public Dictionary<string, Player> players {get; set;} = new ();

    public static string GenerateRoomId(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVXZ0123456789";
        var bytes = new byte[length];

        RandomNumberGenerator.Fill(bytes);

        return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
    }
}