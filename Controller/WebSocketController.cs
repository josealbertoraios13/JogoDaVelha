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
    // Enum que define os tipos de ações suportadas pelo servidor via WebSocket.
    //
    // Cada valor representa uma rota lógica, utilizada para identificar
    // qual funcionalidade do jogo deve ser executada pelo servidor.
    // Esse enum é usado como chave no dicionário de rotas para direcionar
    // a requisição recebida para a função correta.
    public enum WebSocketRouteType
    {
        Create,   // Criação de uma nova sala ou partida
        Join,     // Entrada de um jogador em uma sala existente
        Leave,    // Saída de um jogador da sala
        Move,     // Envio de uma jogada ou movimento
        Message   // Envio de mensagens genéricas (chat ou eventos)
    }

    // Dicionário responsável por mapear cada tipo de rota WebSocket (WebSocketRouteType)
    // para o seu respectivo modelo de execução (WebSocketRouteModel).
    //
    // Cada entrada define:
    // - O tipo de rota (ex: Create, Join)
    // - O tipo do DTO de requisição esperado
    // - O tipo do DTO de resposta retornado
    // - A função que será executada quando essa rota for requisitada
    //
    // Esse dicionário funciona como um sistema de roteamento, permitindo que uma única
    // conexão WebSocket processe diferentes ações do jogo com base no tipo da rota.
    protected readonly Dictionary<WebSocketRouteType, WebSocketRouteModel> socketDictionary = new()
    {
        {WebSocketRouteType.Create, new WebSocketRouteModel(typeof(CreateResquest), typeof(RoomResponse), Application.instance!.Create)},
        {WebSocketRouteType.Join, new WebSocketRouteModel(typeof(JoinRequest), typeof(RoomResponse), Application.instance!.Join)},
        //{WebSocketRouteType.Leave, new WebSocketRouteModel(...)}
        //{WebSocketRouteType.Move, new WebSocketRouteModel(...)}
        //{WebSocketRouteType.Message, new WebSocketRouteModel(...)}
    };

    // Esse método vai ser usado para receber e enviar todos os tipos de requisições de WebSocket
    // Esse método usa o atributo "protected", que só permite o uso do método por esta classe e derivadas
    // Esse método é assíncrono e retorna uma Task, podendo ser aguardado com "await"
    // Esse método precisa de dois parâmetros dos tipos WebSocket e WebSocketRouteType
    // O objeto do tipo WebSocket representa a conexão ativa entre cliente e servidor. Ademais, ele é usado para receber e enviar respostas
    // O enum WebSocketRouteType serve para definir que tipo de função do jogo da velha esta sendo requisitada (ex: Create ou Join)
    protected async Task Handle(WebSocket webSocket, WebSocketRouteType webSocketRouteType)
    {
        // Buffer (porção de memória) de bytes usado para receber mensagens do WebSocket (1 KB por leitura)
        var buffer =  new Byte[1024];

        // Objeto de configuração para serialização e desserialização JSON
        var jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        // Loop principal que processa mensagens enquanto a conexão WebSocket estiver aberta
        while (true)
        {
            // O json do cliente vai se entregue nessa linha em bytes
            var receiveResult = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

            // Esse if serve para verificar se a mensagem recebida é uma solicitação de desconexão
            if(receiveResult.MessageType == WebSocketMessageType.Close)
            {
                // Aqui a conexão é encerrada e o loop é quebrado
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                break;
            }

            // Essa linha converte o buffer de bytes (buffer que contém a mensagem enviada), para string UTF8
            var requestJson = Encoding.UTF8.GetString(buffer, 0,  receiveResult.Count);

            // Aqui é o bloco try (vai tentar realizar todas as tarefas a seguir, caso aconteça alguma exceção o catch vai capturar e transmitir a exceção)
            try
            {
                // Esse bloco "if" pede para que o dicionário tente achar os valores correspondentes ao enum de rotas (WebSocketRouteType)
                if(socketDictionary.TryGetValue(webSocketRouteType, out var socketModel))
                {
                    // Aqui o json(string) "requestJson", é convertido para um requestDto (Data Transfer Object) do tipo definido pela chave do dicionário
                    var requestDto = (IRequest)JsonSerializer.Deserialize(
                        requestJson, 
                        socketModel.request!, 
                        jsonOptions)!;

                    // Aqui a variável ResponseDto(Data Transfer Object), recebe o resultado de uma função específica definida pela chave do dicionário
                    var responseDto = socketModel.func!(requestDto);

                    // Aqui a variável responseJson converte o responseDto para json(string)
                    var responseJson = JsonSerializer.Serialize(
                        responseDto, 
                        socketModel.response!,
                         jsonOptions);

                    // A variável responseBytes recebe o valor de responseJson convertido para valores de bytes                         
                    var responseBytes = Encoding.UTF8.GetBytes(responseJson);
                
                    // Envio da mensagem (responseBytes) de forma assíncrona
                    await webSocket.SendAsync(responseBytes, WebSocketMessageType.Text, true, CancellationToken.None); 
                }
            } // Captura e retorna erros ocorridos durante o processamento da mensagem
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");

                var errorBytes = Encoding.UTF8.GetBytes($"Erro: {ex}");
                await webSocket.SendAsync(errorBytes, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}