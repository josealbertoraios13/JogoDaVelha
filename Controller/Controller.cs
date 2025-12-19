using System.Net;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace controller;

[ApiController]
public class Controller : WebSocketController
{
    [Route("/ws/{route}")]
    public async Task Socket(string route)
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        if (!Enum.TryParse<WebSocketRouteType>(route, true, out var webSocketRouteType))
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();

        switch (webSocketRouteType)
        {
            case WebSocketRouteType.Create:
                await Handle(ws, WebSocketRouteType.Create);
                break;

            case WebSocketRouteType.Join:
                await Handle(ws, WebSocketRouteType.Join);
                break;

            case WebSocketRouteType.Leave:
                break;

            case WebSocketRouteType.Move:
                break;

            case WebSocketRouteType.Message:
                break;

            default:
                await ws.CloseAsync(
                    WebSocketCloseStatus.InvalidPayloadData,
                    "Rota inválida",
                    CancellationToken.None
                );
                break;
        }
    }
}

