using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Ana.Interaction
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _configuration;
        private readonly InteractionService _handler;
        private readonly IServiceProvider _serviceProvider;

        public InteractionHandler(DiscordSocketClient client, IConfiguration configuration, InteractionService handler, IServiceProvider serviceProvider)
        {
            this._client = client;
            this._configuration = configuration;
            this._handler = handler;
            this._serviceProvider = serviceProvider;

        }

        public async Task InitializeAsync()
        {
            _client.Ready += ReadyAsync;

            await _handler.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

            _client.InteractionCreated += HandleInteraction;
        }

        private async Task ReadyAsync()
        {
            await _handler.RegisterCommandsGloballyAsync(true);
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(_client, interaction);

                var result = await _handler.ExecuteCommandAsync(context, _serviceProvider);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


    }
}