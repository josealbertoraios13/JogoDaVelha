namespace model.game;

using model.game.enums;

public class Player
{
    public string id {get; set;}
    public string name {get; set;} = string.Empty;
    public string avatar {get; set;} = string.Empty;
    public Types type {get; set;}
    public int wins {get; set;} = 0;

    public Player(string id, string name, string avatar)
    {
        this.id = id;
        this.name = name;
        this.avatar = avatar;
    }
}