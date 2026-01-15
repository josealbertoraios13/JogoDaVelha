using model.game.enums;

namespace model.game;

public class Game
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

        BlockValues playerValue = TypeToValue(currentTurn.type);

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

    private bool CheckWinner()
    {
        int [][][] winPatterns =
        {
            [[0,0], [0,1], [0,2]],
            [[1,0], [1,1], [1,2]],
            [[2,0], [2,1], [2,2]],

            [[0,0], [1,0], [2,0]],
            [[0,1], [1,1], [2,1]],
            [[0,2], [1,2], [2,2]],

            [[0,0], [1,1], [2,2]],
            [[0,2], [1,1], [2,0]]
        };

        foreach (var pattern in winPatterns)
        {
            var blockPatternA = pattern[0];
            var blockPatternB = pattern[1];
            var blockPatternC = pattern[2];

            if (IsEqual(
                table[blockPatternA[0], blockPatternA[1]],
                table[blockPatternB[0], blockPatternB[1]],
                table[blockPatternC[0], blockPatternC[1]]
            ))
            {
                int[,] winnerBlocks =
                {
                    { blockPatternA[0], blockPatternA[1] },
                    { blockPatternB[0], blockPatternB[1] },
                    { blockPatternC[0], blockPatternC[1] }
                };

                SendWinnerNotification(currentTurn.id, winnerBlocks);
                return true;
            }
        }
        return false;
    }
    
    private bool HasDraw()
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
        table = new BlockValues[3,3];
        currentTurn = playerX;
        SendTable();
        SendCurrentTurn();
        await SetTimeOut();

    }

    private async Task SetTimeOut()
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

    private void SendWinnerNotification(string playerId, int[,] winnerBlocks)
    {
        Console.WriteLine($"Sending winner notification to player: {playerId} with winner blocks: {winnerBlocks}");
    }

    private void SendDrawNotification()
    {
        Console.WriteLine("The game ended in a draw");
    }  

    private BlockValues TypeToValue(PlayerType player)
    {
        return player switch
        {
            PlayerType.X => BlockValues.X,
            PlayerType.O => BlockValues.O,
            _ => BlockValues.Empty,
        };
    }

    private bool IsEqual(BlockValues a, BlockValues b, BlockValues c)
    {
        return a != BlockValues.Empty && a == b && b == c;
    }
}