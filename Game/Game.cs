using System;
using System.Text.Json;
using model.game;
using model.responses;

namespace game;

public class Game
{
    public static List<Player> activePlayers = new();
    public static List<Room> activatedRooms = new();


    public async Task<CreateResponse> Create(Player? player)
    {
        var room = new Room("64712", player!);

        var response = new CreateResponse()
        {
            player = player,
            room = room  
        };
        
        return response;
    }

}