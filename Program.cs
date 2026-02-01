public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy.WithOrigins(
                                  "https://jogo.kaworii.com.br",
                                  "http://jogo.kaworii.com.br",
                                  "http://localhost:5173"
                              )
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
                });

        builder.Services.AddSignalR();

        var app = builder.Build();

        app.UseCors();

        app.MapHub<GameHub>("/GameHub");

        await app.RunAsync();
    }
}
