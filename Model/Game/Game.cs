namespace model.game;

class Game
{
    public string[,] table = new string[3,3];
    public Player? currentTurn;
    public Player? playerX;
    public Player? playerO;

    public Game(Player playerX, Player playerO)
    {
        this.playerX = playerX;
        this.playerO = playerO;
        currentTurn = playerX;
        SendTable();
        SendPlayer(playerX); //envia player com todas as informações
        SendPlayer(playerO);
        SendCurrentTurn();
    }
    
    public string[,] SendTable()
    {
        return this.table;
    }

    public Player SendPlayer(Player player)
    {
        return player.id;
    }

    public string SendCurrentTurn() //envia id do player jogando atualmente
    {
        return currentTurn.id;
    }

    public void MakeMove(string playerId, int x, int y)
    {
        
    }

    public void ChangeTurn()
    {
        
    }

    public void CheckWinner()
    {
        
    }

    public void HasDraw()
    {
        
    }

    public void Reset()
    {
        
    }
}