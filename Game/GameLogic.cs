using System.Data.Common;
using System.Xml;

namespace game;

public class GameLogic
{
    public string[,] table = new string[3, 3];
    //public string currentTurn = playerX;
    public void CleanBoard()
    {
        Array.Clear(table, 0, table.Length);
    }

    public void CurrentTurn(string playerX, string playerO, string currentTurn)
    {
        if (currentTurn == playerX) currentTurn = playerO;
        else currentTurn = playerX;
    }

    //public bool IsPositionEmpty(Block block)
    //{
        //return table[block.x, block.y] == '\0';
    //}

    public bool ValidPlay(string id, string currentTurn)
    {
        return id == currentTurn;
        
    }

    //public string Play(Block block, string type, string id, string currentTurn)
    //{
        //if (IsPositionEmpty(block.x, block.y) && ValidPlay(id, currentTurn))
        //{
            //table[block.x, block.y] = playerType;
            //return "Play successful";
        //}
        //return "Invalid play";
    //}

    public bool HasWinner(string playerType)
    {
        for (int i = 0; i < 3; i++)
        {
            if (table[i, 0] == playerType && table[i, 1] == playerType && table[i, 2] == playerType)
                return true;
            if (table[0, i] == playerType && table[1, i] == playerType && table[2, i] == playerType)
                return true;
        }
        if (table[0, 0] == playerType && table[1, 1] == playerType && table[2, 2] == playerType)
            return true;
        if (table[2, 0] == playerType && table[1, 1] == playerType && table[0, 2] == playerType)
            return true;
        return false;
    }

    public bool Draw()
    {
        foreach (string block in table)
        {
            if (block == "null")
                return false;
        }
        return true;
    }

    public void ShouldEndGame()
    {
        if (Draw() || HasWinner("X") || HasWinner("O"))
        {
            CleanBoard();
        }
    }
}