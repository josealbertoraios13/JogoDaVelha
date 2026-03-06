using System.Security.Cryptography;
using interfaces.responses;
using model.responses;

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

    public static string GenerateRoomId(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVXZ0123456789";
        var bytes = new byte[length];

        RandomNumberGenerator.Fill(bytes);

        return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
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
    
    public IResponse GameResponse(string playerID, int x, int y)
    {
        if (game == null)
            throw new Exception("Game not started");

        var result = game.MakeMove(playerID, x, y);

        if (result.winner != null)
        {
            return new WinnerResponse
            {
                winner = result.winner,
                winnerMoves = result.winnerMoves,
                isDrawEvent = result.isDrawEvent,
                draws = result.draws,
                players = Convert.PlayerListToResponseList(players)
            };
        }
        if (result.isDrawEvent)
        {
            return new WinnerResponse
            {
                winner = null,
                winnerMoves = null,
                isDrawEvent = result.isDrawEvent,
                draws = result.draws,
                players = Convert.PlayerListToResponseList(players)
            };
        }
        return new UpdateTableResponse
        {
            currentTurn = result.currentTurn,
            table = result.table
        };
    }

    public async Task<IResponse> GameReset()
    {
        if (game == null)
            throw new Exception("Game not started");

        var reset = await game.Reset();
        return new UpdateTableResponse
        {
            currentTurn = reset.currentTurn,
            table = reset.table,
        };
    }
}