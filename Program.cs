using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Python.Runtime;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using DiscordCommands = Discord.Commands;
using Discord.Interactions;

using Ana.Interaction;

public partial class Program 
{

    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _services;

    private readonly DiscordSocketConfig _socketConfig = new()
    {
        AlwaysDownloadUsers = false,
        MessageCacheSize = 200
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


// IServiceProvider serviceProvider = null!;

// var builder = new HostBuilder()
//     .ConfigureAppConfiguration(config =>
//     {
//         var configuration = new ConfigurationBuilder()
//             .AddUserSecrets<Program>()
//             .Build();

//         config.AddConfiguration(configuration);
//     })
//     .ConfigureDiscordHost((context, config) =>
//     {
//         config.SocketConfig = new DiscordSocketConfig
//         {
//             AlwaysDownloadUsers = false,
//             MessageCacheSize = 200
//         };

//         config.Token = context.Configuration["DISCORD_BOT_TOKEN"]!;
//     })
//     .UseCommandService((context, config) =>
//     {

//         config.CaseSensitiveCommands = false;
//         config.DefaultRunMode = DiscordCommands.RunMode.Sync;
//     })
//     .ConfigureServices((context, services) =>
//     {
//         services.AddSingleton<DiscordSocketClient>();
//         services.AddSingleton(context.Configuration);
//         services.AddSingleton<InteractionHandler>();
//         services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));


//         serviceProvider = services.BuildServiceProvider();
//     })
//     .UseConsoleLifetime();

// var host = builder.Build();
// var t = ActivatorUtilities.CreateInstance<DiscordSocketClient>(host.Services);

// using (host)
// {
//     var client = serviceProvider.GetRequiredService<DiscordSocketClient>();
//     await serviceProvider.GetRequiredService<InteractionHandler>().InitializeAsync();
//     Console.WriteLine(client);
//     await client.StartAsync();
//     await host.RunAsync();
// }


// var openAiApiKey = configuration["OPENAI_API_KEY"];
// var pythonDllPath = configuration["PYTHON_DLL_PATH"];

// Runtime.PythonDLL = pythonDllPath;
// PythonEngine.Initialize();
// using (Py.GIL())
// {   
//     using(var scope = Py.CreateScope())
//     {
//         scope.Set("person", "Diego");
//     }
//     dynamic module = Py.Import("test_channel");
//     Console.WriteLine(module);
//     dynamic value = module.ReturnValue(openAiApiKey);
//     Console.WriteLine(value);
// }
// PythonEngine.Shutdown();

