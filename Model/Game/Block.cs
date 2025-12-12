using System;

namespace model.game;

// Oculpa pouca memoria
// Para valores simples
public struct Block
{
    int x {get; set;}
    int y {get; set;} 
    string type {get; set;}
}