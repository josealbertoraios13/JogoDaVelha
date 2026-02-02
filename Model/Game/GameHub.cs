using System;
using Microsoft.AspNetCore.SignalR;

// Adicionei Logs para melhor rastreamento das ações dos clientes durante o desenvolvimento.
// TODO: Remover logs antes da versão de produção

public sealed class GameHub : Hub
{
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

    public async Task CreateRoom()
    {
        Console.WriteLine($"🎮 CreateRoom chamado por: {Context.ConnectionId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, "room");
        Console.WriteLine($"✅ Sala criada e cliente {Context.ConnectionId} adicionado ao grupo 'room'");
    }

    public async Task JoinRoom(string room)
    {
        Console.WriteLine($"🚪 JoinRoom chamado - Sala: {room}, Cliente: {Context.ConnectionId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        Console.WriteLine($"✅ Cliente {Context.ConnectionId} entrou na sala '{room}'");
    }

    

    public async Task LeaveRoom(string room)
    {
        Console.WriteLine($"🚪 LeaveRoom chamado - Sala: {room}, Cliente: {Context.ConnectionId}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
        Console.WriteLine($"✅ Cliente {Context.ConnectionId} saiu da sala '{room}'");
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