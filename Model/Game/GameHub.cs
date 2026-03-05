using Microsoft.AspNetCore.SignalR;
using model.requests;
using model.responses;
using interfaces.responses;
using interfaces.roomManager;

public sealed class GameHub : Hub
{   
    private readonly IRoomManager _roomManager;
    public GameHub(IRoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"🔌 Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"🔌 Client disconnected: {Context.ConnectionId}");
        if (exception != null)
        {
            Console.WriteLine($"❌ Connection error: {exception.Message}");
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task<IResponse> CreateRoom(CreateResquest resquest) 
    { 
        var connectionId = Context.ConnectionId; 
        Console.WriteLine($"(CreateRoom) - event called by connection ID: {connectionId}");

        var name = resquest.name; 
        var avatar = resquest.avatar; 

        ValidateData(connectionId, name, avatar);
        
        var room = _roomManager.CreateRoom(connectionId, name, avatar);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, room.id); 

        Console.WriteLine($"(CreateRoom) - Sending event results successfully. Connection ID: {connectionId}");   

        return Convert.RoomToResponse(room);
    }

    public async Task<IResponse> JoinRoom(JoinRequest request)
    {
        var connectionId = Context.ConnectionId; 
        Console.WriteLine($"(JoinRoom) - event called by connection ID: {connectionId}");

        var roomId = request.roomId;
        var name = request.name;
        var avatar = request.avatar;

        ValidateData(connectionId, roomId, name, avatar);

        var room = _roomManager.JoinRoom(roomId, connectionId, name, avatar, out var player);

        await Groups.AddToGroupAsync(connectionId, roomId);

        var playerResponse = Convert.PlayerToResponse(player);

        await Clients.Group(room.id).SendAsync("PlayerJoined", playerResponse);

        Console.WriteLine($"(JoinRoom) - Sending event results successfully. Connection ID: {connectionId}"); 

        return Convert.RoomToResponse(room);
    }
    
    public async Task<IResponse> LeaveRoom(LeaveRequest request)
    {
        var connectionId = Context.ConnectionId; 
        Console.WriteLine($"(LeaveRoom) - event called by connection ID: {connectionId}");

        var roomId = request.roomId;

        ValidateData(connectionId, roomId);

        var leaveRoom = _roomManager.LeaveRoom(roomId, connectionId);

        var room = leaveRoom.room;
        var player = leaveRoom.player;

        var playerResponse = Convert.PlayerToResponse(player);

        await Groups.RemoveFromGroupAsync(connectionId, roomId);
        await Clients.Group(room.id).SendAsync("PlayerLeft", playerResponse);

        Console.WriteLine($"(LeaveRoom) - Sending event results successfully. Connection ID: {connectionId}"); 

        return Convert.RoomToResponse(room);
    }

    public async Task<IResponse> MakeMove(MakeMoveRequest request)
    {
        var connectionId = Context.ConnectionId; 
        Console.WriteLine($"MakeMove event called by connection ID: {connectionId}");

        var roomId = request.roomId;
        var block =  request.Block;

        ValidateData(connectionId, roomId);

        var response = _roomManager.ExecuteMove(roomId, connectionId, block.x, block.y, out var room);

        await Clients.Group(roomId).SendAsync("MakedMove", response);

        if(response is WinnerResponse)
        {
            var resetResponse = await room.GameReset();

            await Clients.Group(room.id).SendAsync("Reset", response);

            Console.WriteLine("It had a draw or a winner.");
        }

        Console.WriteLine($"Sending event results successfully. Connection ID: {connectionId}"); 

        return response;
    }

    public async Task<IResponse> SendMessage(MessageRequest request)
    {
        var connectionId =  Context.ConnectionId;
        Console.WriteLine($"MakeMove event called by connection ID: {connectionId}");
        
        var message = request.message;
        var roomId = request.roomId;

        ValidateData(connectionId, message, roomId);

        var room = _roomManager.GetRoom(roomId);

        var response = new MessageResponse()
        {
            playerID = connectionId,
            message = message,
            createdAt = DateTime.Now
        };

        await Clients.Group(roomId).SendAsync("Message", response);

        Console.WriteLine($"Sending event results successfully. Connection ID: {connectionId}");

        return response;
    }

    public async Task TestConnection()
    {
        Console.WriteLine($"🧪 TestConnection called by: {Context.ConnectionId}");
        await Clients.Caller.SendAsync("ConnectionTest", new { 
            message = "Connection Working", 
            connectionId = Context.ConnectionId,
            timestamp = DateTime.Now
        });
        Console.WriteLine($"✅ Test result send for: {Context.ConnectionId}");
    }

    private void ValidateData(params string[] values)
    {
        foreach(var v in values)
        {
            if(string.IsNullOrWhiteSpace(v))
            {
                throw new HubException("Invalid data");
            }
        }
    }
}