using System;
using model.game;

namespace model.requests;

public interface IRequest;

public record CreateResquest : IRequest
{
    public Player? Player {get; init;}
}

public record JoinRequest : IRequest
{
    public string IdRoom {get; init;} = string.Empty;
    public Player? Player {get; init;}
}

public record LeaveRequest : IRequest
{
    public string IdRoom {get; init;} = string.Empty;
    public Player? Player {get; init;}
}

public record MoveRequest : IRequest
{
    public Block Block {get; init;} 
    public string IdRoom {get; init;} = string.Empty;
    public Player? Player {get; init;}
}
