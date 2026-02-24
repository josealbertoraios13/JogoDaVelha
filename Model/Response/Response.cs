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

public record MakeMove : IResponse
{
    public string [,] ?table {get; init;}
}

public partial record Message : IResponse,  IRequest
{
    public string playerID {get; init;} = string.Empty;
    public string message {get; init;} = string.Empty;
    public DateTime createdAt {get; init;}
}