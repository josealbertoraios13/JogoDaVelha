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

    public void DD()
    {
        Cachorro cachorro = new();
    }


}

// Singleton
public class Cachorro
{
    public static Cachorro _instance;
    public bool taSujo;

    public Cachorro(){
        _instance = this;
    }

    public static void LavarCachorro(Cachorro cachorro)
    {
        cachorro.taSujo = false;
    }
}




