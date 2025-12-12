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

    public void GeneratePlayerId(string id)
    {
        this.id = id;
    }

    public void ValidPlayerName(string name)
    {
        
    }

    public void HasPlayerWon(string type, string id)
    {
        
    }
}

//static - quando pode ser acessado sem instanciar a classe. Métodos gerais para qualquer objeto.
//Singleton - quando a classe só pode ter uma instância. Usado para gerenciar estados globais, como configurações ou conexões de banco de dados.






