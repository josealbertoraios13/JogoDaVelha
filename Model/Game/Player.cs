namespace model.game;

using model.game.enums;

public class Player
{
    public string id {get;} = Guid.NewGuid().ToString();
    public string name {get; set;} = string.Empty;
    public string avatar {get; set;} = string.Empty;
    public Types type {get; set;}
    public int wins {get; set;} = 0;

    public Player(string name, string avatar, Types type)
    {
        this.name = name;
        this.avatar = avatar;
        this.type = type;
    }
}