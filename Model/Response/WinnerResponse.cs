using interfaces.responses;

namespace model.responses;

public record WinnerResponse : IResponse
{
    public string ?winner {get; init;} = string.Empty;
    public int[][] ?winnerMoves {get; init;}
    public bool isDrawEvent {get; init;}
    public int draws {get; init;}
    public List<PlayerResponse> players {get; init;} = new ();
}