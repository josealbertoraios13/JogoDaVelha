namespace model.game;

public class Game
{
    private string[,] table = new string[3,3];
    private Player currentTurn;
    private Player playerX;
    private Player playerO;
    private int draws;

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

        var winnerMoves = CheckWinner();
        if (winnerMoves != null)
        {
            int[][] winnerMovesResponse = WinnerMovesConvert(winnerMoves);
            return new GameResult
            {
                winner = playerId,
                winnerMoves = winnerMovesResponse,
                isDrawEvent = false,
                draws = draws
            };
        }
        else if (HasDraw())
        {
            return new GameResult
            {
                isDrawEvent = true,
                draws = draws
            };
        }
        else
        {
            ChangeTurn();
            string? [][] tableResponse = TableConvert();
            return new GameResult
            {
                currentTurn = currentTurn.id,
                table = tableResponse
            };
        }
    }

    private int[,]? CheckWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if(IsEqual(this.table[i,0], this.table[i,1], this.table[i,2]))
            {
                currentTurn.wins++;
                return new int[,]
                {
                    {i,0},
                    {i,1},
                    {i,2}
                };
                
            };

            if(IsEqual(this.table[0,i], this.table[1,i], this.table[2,i]))
            {
                currentTurn.wins++;
                return new int[,]
                {
                    {0,i},
                    {1,i},
                    {2,i}
                };
                
            }
        }
        if(IsEqual(this.table[0,0], this.table[1,1], this.table[2,2]))
        {   
            currentTurn.wins++;
            return new int[,]
                {
                    {0,0},
                    {1,1},
                    {2,2}
                };
                
        }

        if(IsEqual(this.table[0,2], this.table[1,1], this.table[2,0]))
        {
            currentTurn.wins++;
            return new int[,]
                {
                    {0,2},
                    {1,1},
                    {2,0}
                };
                
        }
        return null;
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
        draws++;
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
    }

    private string?[][] TableConvert()
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

    private int[][] WinnerMovesConvert(int[,] winnerMoves)
    {
        int rows = winnerMoves.GetLength(0);
        int cols = winnerMoves.GetLength(1);

        var winnerMovesResult = new int[rows][];

        for (int i = 0; i < rows; i++)
        {
            winnerMovesResult[i] = new int[cols];

            for (int j = 0; j < cols; j++)
            {
                winnerMovesResult[i][j] = winnerMoves[i, j];
            }
        }
        return winnerMovesResult;
    }

    private bool IsEqual(string a, string b, string c)
    {
        return a != null && a == b && b == c;
    }

    public class GameResult
    {
        public bool isDrawEvent {get; set; }
        public string? winner {get; set; }
        public string? [][] table {get; set; } = default!;
        public int [][] winnerMoves {get; set; } = default!;
        public string currentTurn { get; set; } = string.Empty;
        public int draws {get; set; }
    }
}