using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using game;
using model.game;
using model.requests;

namespace controller;

public class Controller
{
    public readonly Game game;
    public Controller(Game _game)
    {
        game = _game;
    }
    
    public async Task Create(HttpContext context)
    {

        Cachorro._instance.taSujo = false;
        if(context.WebSockets.IsWebSocketRequest){
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            var buffer = new byte[1024];

            while (true)
            {
                var request = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                var requestStr = Encoding.UTF8.GetString(buffer, 0, request.Count);

                Player dto = JsonSerializer.Deserialize<Player>(requestStr)!;

                // Envia o dto para o Game criar a sala e recebe o response adequado
                var response = await game.Create(dto);

                if(request.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                    break;
                }

                var responseEncoding = Encoding.UTF8.GetBytes("Recebi meu nobre");
                await webSocket.SendAsync(responseEncoding, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        else
        {
            context.Response.StatusCode = 400;
        }

    }

    public async Task Join(HttpContext context)
    {
        await Task.Yield();
    }

    public async Task Leave(HttpContext context)
    {
        await Task.Yield();
    }

    public async Task Move(HttpContext context)
    {
        await Task.Yield();
    }

    public async Task Message(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            Console.WriteLine("Connect");

            // Para quê eu vou usar isso??
            var buffer = new byte[1024];
            
            // Pq desse loop??
            while (true)
            {
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                
                if(result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                    Console.WriteLine("Desconnect");
                    break;
                }

                var request = Encoding.UTF8.GetString(buffer, 0, result.Count);

                Console.WriteLine($"Mensagem Recebida: {request} \n Processando...");
                await Task.Delay(10000);

                var response = Encoding.UTF8.GetBytes("Recebi meu nobre");
                await webSocket.SendAsync(response, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
}