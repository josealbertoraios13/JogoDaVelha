using System;
using model.game;

namespace response;
public record CreateResquest
{
    public Player? Player {get; init;}
}

public record JoinRequest
{
    public string IdRoom {get; init;} = string.Empty;
    public Player? Player {get; init;}
}

public record LeaveRequest
{
    public string IdRoom {get; init;} = string.Empty;
    public Player? Player {get; init;}
}

public record MoveRequest
{
    public Block Block {get; init;} 
    public string IdRoom {get; init;} = string.Empty;
    public Player? Player {get; init;}
}
