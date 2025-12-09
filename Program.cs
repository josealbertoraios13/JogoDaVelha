using System;
using game;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<Game>();

        var app = builder.Build();

        app.UseWebSockets();

        app.Map("/ws/create", async (Game g, HttpContext ctx) => g.Create);
        app.Map("/ws/join", async (Game g, HttpContext ctx) => g.Join);
        app.Map("/ws/leave", async (Game g, HttpContext ctx) => g.Leave);
        app.Map("/ws/move", async (Game g, HttpContext ctx) => g.Move);
        app.Map("/ws/message", async (Game g, HttpContext ctx) => g.Message);

        app.Run();
    }
}
