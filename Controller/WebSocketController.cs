using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using application;
using Microsoft.AspNetCore.Mvc;
using model.controller;
using model.requests;
using model.responses;

namespace controller;

public abstract class WebSocketController : ControllerBase
{
    public enum WebSocketRouteType
    {
        Create,
        Join,
        Leave,
        Move,
        Message    }

    protected readonly Dictionary<WebSocketRouteType, WebSocketRouteModel> socketDictionary = new()
    {
        {WebSocketRouteType.Create, new WebSocketRouteModel(typeof(CreateResquest), typeof(RoomResponse), Application.instance!.Create)},
        {WebSocketRouteType.Join, new WebSocketRouteModel(typeof(JoinRequest), typeof(RoomResponse), Application.instance!.Join)},
    };

    protected async Task Handle(WebSocket webSocket, WebSocketRouteType webSocketRouteType)
    {
        var buffer =  new Byte[1024];

        var jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        while (true)
        {
            var receiveResult = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

            if(receiveResult.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                break;
            }

            var requestJson = Encoding.UTF8.GetString(buffer, 0,  receiveResult.Count);

            try
            {
                if(socketDictionary.TryGetValue(webSocketRouteType, out var socketModel))
                {
                    var requestDto = (IRequest)JsonSerializer.Deserialize(
                        requestJson, 
                        socketModel.request!, 
                        jsonOptions)!;

                    var responseDto = socketModel.func!(requestDto);

                    var responseJson = JsonSerializer.Serialize(
                        responseDto, 
                        socketModel.response!,
                         jsonOptions);

                    var responseBytes = Encoding.UTF8.GetBytes(responseJson);
                
                    await webSocket.SendAsync(responseBytes, WebSocketMessageType.Text, true, CancellationToken.None); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");

                var errorBytes = Encoding.UTF8.GetBytes($"Erro: {ex}");
                await webSocket.SendAsync(errorBytes, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}