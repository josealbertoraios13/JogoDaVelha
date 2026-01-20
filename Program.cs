public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR();

        var app = builder.Build();

        app.MapHub<GameHub>(string.Empty);

        await app.RunAsync();
    }
}