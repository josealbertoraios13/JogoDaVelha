using System;

namespace model.game;

public class Player
{
    public string id {get; set;} = string.Empty;
    public string name {get; set;} = string.Empty;
    public string avatar {get; set;} = string.Empty;
    public string type {get; set;} = string.Empty;
    public int wins {get; set;} = 0;

    public Player(string id, string name, string avatar, string type)
    {
        this.id = id;
        this.name = name;
        this.avatar = avatar;
        this.type = type;
    }
}