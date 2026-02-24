using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace model.game;

public class Room
{
    public Game ?game;
    public string id {get; init;} = string.Empty;
    public List<Player> players {get; set;} = new ();

    public void Update()
    {
        TypeDeclare();
        GameStart();
    }

    public void TypeDeclare()
    {
        if(players[0] != null)
            players[0].type = "X";
        
        if(players[1] != null)
            players[1].type = "O";
        
    }

    public void GameStart()
    {
        if(players.Count < 2)
            return;

        if(game == null)
            game = new(players[0], players[1]);
    }

    public static string GenerateRoomId(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVXZ0123456789";
        var bytes = new byte[length];

        RandomNumberGenerator.Fill(bytes);

        return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
    }
}