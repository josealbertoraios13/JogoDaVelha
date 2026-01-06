namespace model.game;

public sealed class Block
{
    public int x {get; set;}
    public int y {get; set;}
    public string type {get; set;} = string.Empty;
    
    public bool Valid()
    {
        return ((x && y < 3) && (type == "X" || "O"));
    }
}
