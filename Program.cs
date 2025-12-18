using application;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        Application game = new();
        
        var app = builder.Build();

        app.UseWebSockets();

        app.MapControllers();

        await app.RunAsync();
    }
}