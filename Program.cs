using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Python.Runtime;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using DiscordCommands = Discord.Commands;
using Discord.Interactions;
using Discord;

using Ana.Service;
using Ana.Interaction;

public partial class Program 
{

    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _services;

    private readonly DiscordSocketConfig _socketConfig = new()
    {
        AlwaysDownloadUsers = false,
        GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
    };

    public Program()
    {
        _configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        _services = new ServiceCollection()
            .AddSingleton(_configuration)
            .AddSingleton(_socketConfig)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<InteractionHandler>()
            .AddSingleton<OpenAiService>()
            .BuildServiceProvider();
    }

    static void Main(string[] args) => new Program().RunAsync()
        .GetAwaiter()
        .GetResult();

    public async Task RunAsync()
    {
        var client = _services.GetRequiredService<DiscordSocketClient>();
        await _services.GetRequiredService<InteractionHandler>()
            .InitializeAsync();
        
        await client.LoginAsync(Discord.TokenType.Bot, _configuration["DISCORD_BOT_TOKEN"]);
        await client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }
}


