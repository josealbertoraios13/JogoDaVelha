using System;
using System.Threading.Tasks;
using controller;
using game;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<Game>();
        builder.Services.AddSingleton<Controller>();

        var app = builder.Build();

        app.UseWebSockets();

        Game game = new();
        Controller controller = new(game);

        app.Map("/ws/create", async (HttpContext context) => await controller.Create(context));
        app.Map("/ws/join", async (HttpContext context) => await controller.Join(context));
        app.Map("/ws/leave", async (HttpContext context) => await controller.Leave(context));
        app.Map("/ws/move", async (HttpContext context) => await controller.Move(context));
        app.Map("/ws/message", async (HttpContext context) => await controller.Message(context));

        await app.RunAsync();
    }
}
