using Discord;
using Discord.Interactions;
using System;
using System.Threading.Tasks;

using Ana.Interaction;
using Ana.Service;

namespace Ana.Modules
{
    public class CommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; } = null!;

        private InteractionHandler _handler;
        private readonly OpenAiService _openAiService;

        public CommandModule(InteractionHandler handler, OpenAiService openAiService)
        {
            _handler = handler;
            _openAiService = openAiService;
        }

        [SlashCommand("ping", "Pings the bot and returns its latency.")]
        public async Task GreetUserAsync()
            => await RespondAsync(text: $":ping_pong: It took me {Context.Client.Latency}ms to respond to you!", ephemeral: true);

        [SlashCommand("test", "Pings the bot and returns its latency.")]
        public async Task TestCommand()
            => await RespondAsync(text: "This is a test!", ephemeral: true);

        [UserCommand("greet")]
        public async Task GreetUserAsync(IUser user)

            => await RespondAsync(text: $":wave: {Context.User} said hi to you, <@{user.Id}>!");
        
        [SlashCommand("echo", "Repeat the input")]
        public async Task Echo(string echo, [Summary(description: "mention the user")] bool mention = false)
        {

            await RespondAsync(echo + (mention ? Context.User.Mention : string.Empty));
        }
        
        [SlashCommand("question", "Ask Ana something related to Totem")]
        public async Task Question(string input) 
        {
            await DeferAsync();
            var answer = await _openAiService.RunQuestionQuery(input);
            await FollowupAsync(text: answer, ephemeral: true);
        }
            


    }
}