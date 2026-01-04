namespace model.game;

public class Player : IdManager
{
    public string id {get; init;} = string.empty;
    public string name {get; set;} = string.Empty;
    public string avatar {get; set;} = string.Empty;
    public string type {get; set;} = string.Empty;
    public int wins {get; set;} = 0;

    public Player(string name, string avatar, string type)
    {
        this.id = GenerateCode();
        this.name = name;
        this.avatar = avatar;
        this.type = type;
    }
}









