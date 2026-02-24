using System;
using model.game;
using model.responses;

namespace model.requests;

public interface IRequest;

public record CreateResquest : IRequest
{
    public string name {get; init;} = string.Empty;
    public string avatar {get; init;} = string.Empty;
}

public record JoinRequest : IRequest
{
    public string IdRoom {get; init;} = string.Empty;
    public string name {get; init;} = string.Empty;
    public string avatar {get; init;} = string.Empty;
}

public record LeaveRequest : IRequest
{
    public string IdRoom {get; init;} = string.Empty;
    public Player? Player {get; init;}
}

public record MakeMoveRequest : IRequest
{
    public Block Block {get; init;} 
    public string IdRoom {get; init;} = string.Empty;
}

public partial record Message : IResponse,  IRequest
{
    public string playerID {get; init;} = string.Empty;
    public string message {get; init;} = string.Empty;
    public DateTime createdAt {get; init;}
}