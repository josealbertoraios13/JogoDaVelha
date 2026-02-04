using System;
using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using model.game;
using model.game.enums;
using model.requests;
using model.responses;

// Adicionei Logs para melhor rastreamento das ações dos clientes durante o desenvolvimento.
// TODO: Remover logs antes da versão de produção

public sealed class GameHub : Hub
{   
    private static Dictionary<string, Room> activatedRooms = new();
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
        var name = resquest.name; 
        if (string.IsNullOrWhiteSpace(name)) 
            throw new HubException("Name can't be null or empty"); 
            
        var avatar = resquest.avatar; 
        if (string.IsNullOrWhiteSpace(avatar)) 
            throw new HubException("Avatar can't be null or empty"); 
            
        var id = Context.ConnectionId; 
        
        var player = new Player(id, name, avatar); 
        
        player.type = Types.X; 
        
        var room = new Room(player); 

        activatedRooms.Add(room.id, room);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, room.id);    

        return new RoomResponse() { room = room }; 
    }

    public async Task<IResponse> JoinRoom(JoinRequest request)
    {
        var idRoom = request.IdRoom;
        if(string.IsNullOrWhiteSpace(idRoom))
            throw new HubException("Id room can't be null or empty");

        var name = request.name;
        if(string.IsNullOrWhiteSpace(name))
            throw new HubException("Name can't be null or empty");

        var avatar = request.avatar;
        if(string.IsNullOrWhiteSpace(avatar))
            throw new HubException("Avatar can't be null or empty");

        if(activatedRooms.TryGetValue(idRoom, out var room))
        {
            if(room.Players.Count >= 2)
                throw new HubException("This room is full");

            var id = Context.ConnectionId;
            var player = new Player(id, name, avatar); 

            player.type = Types.O; 

            room.Players.Add(player);

            await Groups.AddToGroupAsync(id, room.id);

            await Clients.Group(room.id).SendAsync("PlayerJoined", new RoomResponse { room = room });

            return new RoomResponse() { room = room }; 
        }

        throw new HubException("This Room does not exist");
    }

    public async Task LeaveRoom(string room)
    {
        // Próximo para ser implementado
    }

    public Task MakeMove(int x, int y)
    {
        Console.WriteLine($"🎯 MakeMove chamado - Posição: ({x}, {y}), Cliente: {Context.ConnectionId}");
        // Lógica do jogo será implementada aqui
        Console.WriteLine($"✅ Jogada registrada na posição ({x}, {y})");
        return Task.CompletedTask;
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