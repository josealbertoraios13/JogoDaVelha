using System;
using model.game;
using model.requests;

namespace model.responses;

public interface IResponse;

public record RoomResponse : IResponse
{
    public Room? room {get; init;}
}

public record PlayerResponse : IResponse
{
    public Player? player {get; init;}
}

public record MakeMoveResponse : IResponse
{
    public string currentTurn {get; init;} = string.Empty;
    public string?[][] ?table {get; init;}

}

public record WinnerResponse : IResponse
{
    public string winner {get; init;} = string.Empty;
    public string?[][] ?winnerMoves {get; init;}
    public bool isDrawEvent {get; init;}
    public int draws {get; init;}
    public List<Player> players {get; init;} = new ();

}

public partial record Message : IResponse,  IRequest
{
    public string playerID {get; init;} = string.Empty;
    public string message {get; init;} = string.Empty;
    public DateTime createdAt {get; init;}
}