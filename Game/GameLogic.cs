using System.Data.Common;
using System.Xml;
using model.game;

namespace game;

public class GameLogic
{
    public string[,] table = new string[3, 3];
    public Player? currentTurn;

    public void CleanBoard()
    {
        Array.Clear(table, 0, table.Length);
    }

    public void SetInitialTurn(Room room)
    {
        currentTurn = room.playerX;
    }

    public void ChangeTurn(Room room)
    {
        if (currentTurn == room.playerX) currentTurn = room.playerO;
        currentTurn = room.playerX;
    }

    //public bool IsPositionEmpty(Block block)
    //{
        //return table[block.x, block.y] == "null";
    //}

    //public string Play(Block block, string type, string id, string currentTurn)
    //{
        //if (IsPositionEmpty(block.x, block.y) && ValidPlay(id, currentTurn))
        //{
            //table[block.x, block.y] = playerType;
            //return "Play successful";
        //}
        //return "Invalid play";
    //}

    public bool HasWinner()
    {   
        if (currentTurn != null)
            for (int i = 0; i < 3; i++)
            {
                if (table[i, 0] == currentTurn.type && table[i, 1] == player.type && table[i, 2] == player.type)
                    return true;
                if (table[0, i] == player.type && table[1, i] == player.type && table[2, i] == player.type)
                    return true;
            }
            if (table[0, 0] == player.type && table[1, 1] == player.type && table[2, 2] == player.type)
                return true;
            if (table[2, 0] == player.type && table[1, 1] == player.type && table[0, 2] == player.type)
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

    public void ShouldEndGame(Room room)
    {
        if (Draw() || HasWinner(room.playerX.type) || HasWinner(room.playerX.type))
        {
            CleanBoard();
        }
    }
}