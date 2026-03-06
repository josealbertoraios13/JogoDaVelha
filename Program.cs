using interfaces.roomManager;
using model.services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

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

builder.Services.AddSingleton<IRoomManager, RoomManager>();

var app = builder.Build();

app.UseRouting();

app.UseCors();

app.MapHub<GameHub>("/GameHub");

Console.WriteLine("Initial setup completed successfully!");

await app.RunAsync();