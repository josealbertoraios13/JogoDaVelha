namespace model.game;

using model.game.enums;

public class Player : IdManager
{
    public string id {get; init;} = string.Empty;
    public string name {get; set;} = string.Empty;
    public string avatar {get; set;} = string.Empty;
    public PlayerType type {get; set;}
    public int wins {get; set;} = 0;

    public Player(string name, string avatar, PlayerType type)
    {
        this.id = GenerateCode();
        this.name = name;
        this.avatar = avatar;
        this.type = type;
    }
}









