using System;
using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using model.game;
using model.requests;
using model.responses;

// Adicionei Logs para melhor rastreamento das ações dos clientes durante o desenvolvimento.
// TODO: Remover logs antes da versão de produção

public sealed class GameHub : Hub
{   
    private static ConcurrentDictionary<string, Room> activatedRooms = new();
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"🔌 Cliente conectado: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"🔌 Cliente desconectado: {Context.ConnectionId}");
        if (exception != null)
        {
            Console.WriteLine($"❌ Erro na desconexão: {exception.Message}");
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task<IResponse> CreateRoom(CreateResquest resquest) 
    { 
        var idConnection = Context.ConnectionId; 
        Console.WriteLine($"CreateRoom event called by connection ID: {idConnection}");

        var name = resquest.name; 
        if (string.IsNullOrWhiteSpace(name)) 
            throw new HubException("Name can't be null or empty"); 
            
        var avatar = resquest.avatar; 
        if (string.IsNullOrWhiteSpace(avatar)) 
            throw new HubException("Avatar can't be null or empty"); 
            
        
        var player = new Player(idConnection, name, avatar); 
        
        var room = new Room()
        {
            id = Room.GenerateRoomId()
        };

        room.players.Add(player);

        activatedRooms.TryAdd(room.id, room);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, room.id); 

        Console.WriteLine($"Sending event results successfully. Connection ID: {idConnection}");   

        return new RoomResponse() { room = room }; 
    }

    public async Task<IResponse> JoinRoom(JoinRequest request)
    {
        var idConnection = Context.ConnectionId; 
        Console.WriteLine($"JoinRoom event called by connection ID: {idConnection}");

        var idRoom = request.IdRoom;
        if(string.IsNullOrWhiteSpace(idRoom))
            throw new HubException("Id room can't be null or empty");

        var name = request.name;
        if(string.IsNullOrWhiteSpace(name))
            throw new HubException("Name can't be null or empty");

        var avatar = request.avatar;
        if(string.IsNullOrWhiteSpace(avatar))
            throw new HubException("Avatar can't be null or empty");

        if(!activatedRooms.TryGetValue(idRoom, out var room))
            throw new HubException("This Room does not exist");

        Player ?player;
        lock (room)
        {
            if(room.players.Count >= 2)
                throw new HubException("This room is full");

            player = room.players.FirstOrDefault(p => p.id == idConnection);
            if(player != null)
                throw new HubException("This player is already in the room");

            player = new Player(idConnection, name, avatar);

            room.players.Add(player);

            room.Update();
        }

        await Groups.AddToGroupAsync(idConnection, room.id);

        await Clients.Group(room.id).SendAsync("PlayerJoined", new PlayerResponse() {player = player});

        Console.WriteLine($"Sending event results successfully. Connection ID: {idConnection}"); 

        return new RoomResponse() { room = room }; 
    }
    
    public async Task<IResponse> LeaveRoom(string idRoom)
    {
        var idConnection = Context.ConnectionId; 
        Console.WriteLine($"JoinRoom event called by connection ID: {idConnection}");

        if(string.IsNullOrEmpty(idRoom))
            throw new HubException("idRoom can't be null");

        if(!activatedRooms.TryGetValue(idRoom, out var room))
            throw new HubException("This room does not exist");

        Player? player;
        lock(room)
        {
            player = room.players.FirstOrDefault(p => p.id == idConnection);
            if(player == null)
                throw new HubException("This player is not in this room");

            room.players.Remove(player);

            if(room.players.Count == 0)
                if(activatedRooms.TryRemove(idRoom, out var removedRoom))
                    Console.WriteLine($"this {removedRoom} was removed"); // Se Quiser posso enviar isso, mas acredito ser desnecessário
        }

        await Groups.RemoveFromGroupAsync(idConnection, room.id);

        await Clients.Group(room.id).SendAsync("PlayerLeft", new PlayerResponse() {player = player});

        Console.WriteLine($"Sending event results successfully. Connection ID: {idConnection}"); 

        return new RoomResponse() { room = room }; 
    }

    public async Task<IResponse> MakeMove(MakeMoveRequest request)
    {
        var idConnection = Context.ConnectionId; 
        Console.WriteLine($"JoinRoom event called by connection ID: {idConnection}");

        var idRoom = request.IdRoom;
        if(string.IsNullOrEmpty(idRoom))
            throw new HubException("idRoom can't be null");

        if(!activatedRooms.TryGetValue(idRoom, out var room))
            throw new HubException("This room does not exist");

        var block =  request.Block;

        Game ?game;
        lock (room)
        {
            game = room.game;
        }

        if(game == null)
            throw new HubException("Game not started");

        var response = await room.GameResponse(idConnection, x, y); //Poderia iniciar no Room.GameResponse?

        await Clients.Group(room.id).SendAsync("MakedMove", response);

        return response;
    }

    public async Task<IResponse> Message(model.requests.Message request)
    {
        var message = request.message;
        if(string.IsNullOrEmpty(message))
            throw new HubException("Message is null");

        var response = new model.responses.Message()
        {
            playerID = Context.ConnectionId,
            message = message,
            createdAt = request.createdAt
        };

        //await Clients.Group().SendAsync

        return response;
    }

    public Task ResetGame()
    {
        Console.WriteLine($"🔄 ResetGame chamado por: {Context.ConnectionId}");
        // Lógica de reset será implementada aqui
        Console.WriteLine($"✅ Jogo resetado");
        return Task.CompletedTask;
    }

    public async Task TestConnection()
    {
        Console.WriteLine($"🧪 TestConnection chamado por: {Context.ConnectionId}");
        await Clients.Caller.SendAsync("ConnectionTest", new { 
            message = "Conexão funcionando!", 
            connectionId = Context.ConnectionId,
            timestamp = DateTime.Now
        });
        Console.WriteLine($"✅ Resposta de teste enviada para {Context.ConnectionId}");
    }
}