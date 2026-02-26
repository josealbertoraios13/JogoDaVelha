
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

var app = builder.Build();

app.UseRouting();

app.UseCors();

app.MapHub<GameHub>("/GameHub");

await app.RunAsync();