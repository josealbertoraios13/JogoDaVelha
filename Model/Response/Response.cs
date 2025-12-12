using System;
using model.game;

namespace model.responses;
public record CreateResponse
{
    public Room? room {get; init;}
    public Player? player {get; init;}
}

public record Winner
{
    
}

public record Messages
{
    
}