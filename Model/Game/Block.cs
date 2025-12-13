using System;

namespace model.game;

public struct Block
{
    public int x {get; set;}
    public int y {get; set;}
    public string type {get; set;} = string.Empty;
}

public override bool Valid()
{
    return ((x && y < 3) && (type == "X" || "O"));
}