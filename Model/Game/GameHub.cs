using System;
using Microsoft.AspNetCore.SignalR;

public sealed class GameHub : Hub
{
    public async Task CreateRoom()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "room");
    }

    public async Task JoinRoom(string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
    }

    public async Task LeaveRoom(string room)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
    }

}