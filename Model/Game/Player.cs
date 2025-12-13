namespace model.game;

public class Player : ModelGeralStructure
{
    public string name {get; set;} = string.Empty;
    public string avatar {get; set;} = string.Empty;
    public string type {get; set;} = string.Empty;
    public int wins {get; set;} = 0;

    public Player(string name, string avatar, string type)
    {
        GenerateId();
        this.name = name;
        this.avatar = avatar;
        this.type = type;
    }
<<<<<<< HEAD
}
=======

    public override bool Valid()
    {
        return !string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(name) && (type == "X" || type == "O");
    }
}








>>>>>>> 89f3303 (feat(*): salvando alterações)
