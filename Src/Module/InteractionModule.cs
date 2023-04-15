using Discord;
using Discord.Interactions;
using System;
using System.Threading.Tasks;

using Ana.Interaction;

namespace Ana.Modules
{
    public class CommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; } = null!;

        private InteractionHandler _handler;

        public CommandModule(InteractionHandler handler)
        {
            _handler = handler;
        }

        [SlashCommand("ping", "Pings the bot and returns its latency.")]
        public async Task GreetUserAsync()
            => await RespondAsync(text: $":ping_pong: It took me {Context.Client.Latency}ms to respond to you!", ephemeral: true);

        [UserCommand("greet")]
        public async Task GreetUserAsync(IUser user)

            => await RespondAsync(text: $":wave: {Context.User} said hi to you, <@{user.Id}>!");
        
        [SlashCommand("echo", "Repeat the input")]
        public async Task Echo(string echo, [Summary(description: "mention the user")] bool mention = false)
            => await RespondAsync(echo + (mention ? Context.User.Mention : string.Empty));
    }
}