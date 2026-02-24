using model.game.enums;
using model.responses;

namespace model.game;

public class Game
{
    private string[,] table = new string[3,3];
    private Player currentTurn;
    private Player playerX;
    private Player playerO;
    private int draws = 0;

    public Game(Player playerX, Player playerO)
    {
        this.playerX = playerX;
        this.playerO = playerO;
        currentTurn = playerX;
        SendTable();
        SendPlayer(playerX);
        SendPlayer(playerO);
        SendCurrentTurn();
    }

    public async Task<IResponse> MakeMove(string playerId, int x, int y)
    {
        if (currentTurn.id != playerId)
        {
            throw new Exception("Not your turn");
        }
        
        if (x < 0 || x > 2 || y < 0 || y > 2)
        {
            throw new Exception("Invalid position");
        }

        if (table[x,y] != null)
        {
            throw new Exception("Block already occupied");
        }

        this.table[x,y] = currentTurn.type;

        SendTable();
        if (CheckWinner() || HasDraw())
        {
            await Reset();
        }
        else
        {
            ChangeTurn();
            SendCurrentTurn();
        }
    }

    private bool CheckWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if(IsEqual(this.table[i,0], this.table[i,1], this.table[i,2]))
            {
                int[,] winnerBlocks = new int[,]
                {
                    {i,0},
                    {i,1},
                    {i,2}
                };
                SendWinnerNotification(currentTurn.id, winnerBlocks);
                return true;
            };

            if(IsEqual(this.table[0,i], this.table[1,i], this.table[2,i]))
            {
                int[,] winnerBlocks = new int[,]
                {
                    {0,i},
                    {1,i},
                    {2,i}
                };
                SendWinnerNotification(currentTurn.id, winnerBlocks);
                return true;
            }
        }
        if(IsEqual(this.table[0,0], this.table[1,1], this.table[2,2]))
        {
            int[,] winnerBlocks = new int[,]
                {
                    {0,0},
                    {1,1},
                    {2,2}
                };
                SendWinnerNotification(currentTurn.id, winnerBlocks);
                return true;
        }

        if(IsEqual(this.table[0,2], this.table[1,1], this.table[2,0]))
        {
            int[,] winnerBlocks = new int[,]
                {
                    {0,2},
                    {1,1},
                    {2,0}
                };
                SendWinnerNotification(currentTurn.id, winnerBlocks);
                return true;
        }
        return false;
    }

    private bool HasDraw()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (this.table[i,j] == null)
                {
                    return false;
                    
                }
            }
        }
        SendDrawNotification();
        return true;
    }

    private void ChangeTurn()
    {
        if (this.currentTurn == this.playerX)
        {
            this.currentTurn = this.playerO;
            return;
        }
        this.currentTurn = this.playerX;
    }

    private async Task Reset()
    {
        await Task.Delay(1000);
        table = new string[3,3];
        currentTurn = playerX;
        SendTable();
        SendCurrentTurn();
    }

    public string[,] SendTable()
    {
        return table;
    }

    public string SendPlayer(Player player)
    {
        return player.id;
    }

    public string SendCurrentTurn()
    {
        return currentTurn.id;
    }

    private void SendWinnerNotification(string playerId, int[,] winnerBlocks)
    {
        currentTurn.wins++;
        Console.WriteLine($"Sending winner notification to player: {playerId} with winner blocks: {winnerBlocks}. {playerId} has {currentTurn.wins} wins now");
    }

    private void SendDrawNotification()
    {
        draws++;
        Console.WriteLine($"The game ended in a draw. Draws equals {draws}");
    }  

    private bool IsEqual(string a, string b, string c)
    {
        return a != null && a == b && b == c;
    }
}