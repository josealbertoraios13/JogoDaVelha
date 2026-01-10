using model.game.enums;

namespace model.game;

class Game
{
    public BlockValues[,] table = new BlockValues[3,3];
    public Player currentTurn;
    public Player playerX;
    public Player playerO;

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
    
    public async Task MakeMove(string playerId, int x, int y)
    {
        if (currentTurn.id != playerId)
        {
            throw new Exception("Not your turn");
        }
        
        if (table[x,y] != BlockValues.Empty)
        {
            throw new Exception("Block already occupied");
        }

        BlockValues playerValue = TypeToBlock(currentTurn.type);

        this.table[x,y] = playerValue;
        SendTable();
        if (CheckWinner() || HasDraw())
        {
            await Reset();
            return;
        } 
        ChangeTurn();
        SendCurrentTurn();
    }

    public bool CheckWinner()
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
    
    public bool HasDraw()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (this.table[i,j] == BlockValues.Empty)
                {
                    return false;
                    
                }
            }
        }
        SendDrawNotification();
        return true;
    }
    
    public void ChangeTurn()
    {
        if (this.currentTurn == this.playerX)
        {
            this.currentTurn = this.playerO;
            return;
        }
        this.currentTurn = this.playerX;
    }
    
    public async Task Reset()
    {
        table = new BlockValues[3,3];
        currentTurn = playerX;
        SendTable();
        SendCurrentTurn();
        await SetTimeOut();

    }

    public async Task SetTimeOut()
    {
        Console.WriteLine("Game has been reset.");
        for (int i = 10; i >= 0; i--)
        {
            Console.WriteLine(i);
            await Task.Delay(1000);
        }
    }

    public BlockValues[,] SendTable()
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
    
    public void SendWinnerNotification(string playerId, int[,] winnerBlocks)
    {
        Console.WriteLine($"Sending winner notification to player: {playerId} with winner blocks: {winnerBlocks}");
    }

    public void SendDrawNotification()
    {
        Console.WriteLine("The game ended in a draw");
    }    
    
    public BlockValues TypeToBlock(PlayerType player)
    {
        return player switch
        {
            PlayerType.X => BlockValues.X,
            PlayerType.O => BlockValues.O,
            _ => BlockValues.Empty,
        };
    }
    
    public bool IsEqual(BlockValues a, BlockValues b, BlockValues c)
    {
        return a != BlockValues.Empty && a == b && b == c;
    }
}