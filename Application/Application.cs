using model.game;
using model.requests;
using model.responses;

namespace application;

public class Application
{
    public static Application? instance = null;
    public static List<Player> activePlayers = new();
    public static List<Room> activatedRooms = new();

    public Application()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public IResponse Create(IRequest? request)
    {
        if(request is not CreateResquest)
            throw new ArgumentException(nameof(request));

        var createRequest = request as CreateResquest;

        if (createRequest is null)
            throw new ArgumentNullException(nameof(createRequest));

        if(createRequest.Player is null)
            throw new ArgumentNullException(nameof(createRequest.Player));

        var player = createRequest.Player;
        var room = new Room(player!);

        activatedRooms.Add(room);

        return new RoomResponse() 
        {
            room = room
        };
    }

    public IResponse Join(IRequest? request)
    {
        if(request is not JoinRequest)
            throw new ArgumentException(nameof(request));

        var joinRequest = request as JoinRequest;

        if(joinRequest is null)
            throw new ArgumentNullException(nameof(joinRequest));

        if (string.IsNullOrWhiteSpace(joinRequest.IdRoom))
            throw new ArgumentException("Id da sala não pode ser nulo ou vazio");

        if(joinRequest.Player is null)
            throw new ArgumentNullException(nameof(joinRequest.Player));

        var room = activatedRooms.FirstOrDefault(r => r.id == joinRequest.IdRoom);

        if(room is null)
            throw new KeyNotFoundException("Sala não encontrada");

        room.Players.Add(joinRequest.Player);

        return new RoomResponse()
        {
            room = room
        };
    }

    public IResponse Leave (IRequest? joinRequest) => throw new NotImplementedException();
    public IResponse Move (IRequest? joinRequest) => throw new NotImplementedException();
    public IResponse Message (IRequest? joinRequest) => throw new NotImplementedException();
}