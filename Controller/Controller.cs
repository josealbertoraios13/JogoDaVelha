using System.Net;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace controller;

[ApiController]
public class Controller : WebSocketController
{
    // Endpoint responsável por iniciar conexões WebSocket.
    // A rota define qual ação (WebSocketRouteType) será executada pelo servidor.
    [Route("/ws/{route}")]
    public async Task Socket(string route)
    {
        // Verifica se a requisição recebida é do tipo WebSocket
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            // Caso não seja, retorna erro 400 (Bad Request)
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        // Converte o valor da rota (string) para o enum WebSocketRouteType,
        // ignorando diferença entre maiúsculas e minúsculas
        if (!Enum.TryParse<WebSocketRouteType>(route, true, out var webSocketRouteType))
        {
            // Retorna erro caso a rota informada não seja válida
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        // Aceita a conexão WebSocket e cria o canal de comunicação
        var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();

        // Direciona a conexão para o manipulador correspondente à rota solicitada
        switch (webSocketRouteType)
        {
            case WebSocketRouteType.Create:
                await Handle(ws, WebSocketRouteType.Create);
                break;

            case WebSocketRouteType.Join:
                await Handle(ws, WebSocketRouteType.Join);
                break;

            case WebSocketRouteType.Leave:
                // Rota ainda não implementada
                break;

            case WebSocketRouteType.Move:
                // Rota ainda não implementada
                break;

            case WebSocketRouteType.Message:
                // Rota ainda não implementada
                break;

            default:
                // Encerra a conexão caso a rota seja inválida ou não suportada
                await ws.CloseAsync(
                    WebSocketCloseStatus.InvalidPayloadData,
                    "Rota inválida",
                    CancellationToken.None
                );
                break;
        }
    }
}

