using System.Collections.Concurrent;
using interfaces.responses;
using interfaces.roomManager;
using Microsoft.AspNetCore.SignalR;
using model.game;

namespace model.services;

public class RoomManager : IRoomManager
{
    private static ConcurrentDictionary<string, Room> activatedRooms = new();
    public Room CreateRoom(string connectionId, string name, string avatar) 
    { 
        var player = new Player(connectionId, name, avatar); 
        
        var room = new Room()
        {
            id = Room.GenerateRoomId()
        };

        room.players.Add(player);

        activatedRooms.TryAdd(room.id, room);
        
        return room;
    }

    public Room JoinRoom(string roomId, string connectionId, string name, string avatar, out Player newPlayer)
    {
        var room = GetRoom(roomId);

        Player ?player;
        lock (room)
        {
            if(room.players.Count >= 2)
                throw new HubException("This room is full");

            player = room.players.FirstOrDefault(p => p.id == connectionId);
            if(player != null)
                throw new HubException("This player is already in the room");

            player = new Player(connectionId, name, avatar);

            room.players.Add(player);

            room.Update();
        }

        newPlayer = player!;
        return room;
    }

    public (Room room, Player player) LeaveRoom(string roomId, string connectionId)
    {
        var room = GetRoom(roomId);

        Player? player;
        lock(room)
        {
            player = room.players.FirstOrDefault(p => p.id == connectionId);
            if(player == null)
                throw new HubException("This player is not in this room");

            room.players.Remove(player);

            if(room.players.Count == 0)
                if(activatedRooms.TryRemove(roomId, out var removedRoom))
                    Console.WriteLine($"this {removedRoom} was removed");
        }

        return (room, player);
    }

    public IResponse ExecuteMove(string roomId, string connectionId, int x, int y, out Room selectedRoom)
    {
        var room = GetRoom(roomId);

        lock (room)
        {
            selectedRoom = room;
            return room.GameResponse(connectionId, x, y);
        }
    }

    public Room GetRoom(string roomId)
    {
        if(!activatedRooms.TryGetValue(roomId, out var room))
            throw new HubException("This Room does not exist");

        return room;
    }
}