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
    }

    public GameResult MakeMove(string playerId, int x, int y)
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

        table[x,y] = currentTurn.type;

        if (CheckWinner())
        {
            return new GameResult
            {
                
            };
        }
        ChangeTurn();
    }

    private int[,]? CheckWinner()
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
            };

            if(IsEqual(this.table[0,i], this.table[1,i], this.table[2,i]))
            {
                int[,] winnerBlocks = new int[,]
                {
                    {0,i},
                    {1,i},
                    {2,i}
                };
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
        return true;
    }

    private void ChangeTurn()
    {
        if (currentTurn == playerX)
        {
            currentTurn = playerO;
            return;
        }
        currentTurn = playerX;
    }

    private async Task Reset()
    {
        await Task.Delay(1000);
        table = new string[3,3];
        currentTurn = playerX;
        TableConvert();
        SendCurrentTurn();
    }

    public string?[][] TableConvert()
    {
        int rows = table.GetLength(0);
        int cols = table.GetLength(1);

        var tableResult = new string?[rows][];

        for (int i = 0; i < rows; i++)
        {
            tableResult[i] = new string?[cols];

            for (int j = 0; j < cols; j++)
            {
                tableResult[i][j] = table[i, j];
            }
        }
        return tableResult;
    }

    public string SendPlayer(Player player)
    {
        return player.id;
    }

    public string SendCurrentTurn()
    {
        return currentTurn.id;
    }

    public int SendDraw(bool draw)
    {
        if (draw)
        {
            return draws++;
        }
        return draws;
    }

    private bool IsEqual(string a, string b, string c)
    {
        return a != null && a == b && b == c;
    }

    public class GameResult()
    {
        public bool isDraw {get; set; }
        public string? winner {get; set; }
        public string? [][] table {get; set; } = default!;
        public string? [][] winnerMoves {get; set; } = default!;
        public string currentTurnId { get; set; } = string.Empty;
    }
}