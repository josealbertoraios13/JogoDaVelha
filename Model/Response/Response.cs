using System;
using model.game;

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

public record Winner : IResponse
{
    
}

public record Messages : IResponse
{
    
}