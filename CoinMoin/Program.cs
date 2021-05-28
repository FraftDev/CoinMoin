using System;
using System.Threading.Tasks;
using CoinMoin.Commands;
using CoinMoin.Config;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoinMoin
{
    class Program
    {
        public readonly EventId BotEventId = new EventId(1, "Instance-#001");

        public DiscordClient Client { get; set; }
        public CommandsNextExtension Commands { get; set; }

        static void Main(string[] args)
        {
            var program = new Program();
            program.RunBotAsync().GetAwaiter().GetResult();
        }

        public async Task RunBotAsync()
        {
            Configuration config = Configuration.LoadFromFile();

            var discordConfig = new DiscordConfiguration
            {
                Token = config.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            this.Client = new DiscordClient(discordConfig);

            this.Client.Ready += Client_Ready;
            this.Client.GuildAvailable += Client_GuildAvailable;
            this.Client.ClientErrored += Client_ClientErrored;

            var commandConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new[] { config.Prefix },
                EnableDms = true,
                EnableMentionPrefix = true
            };

            this.Commands = this.Client.UseCommandsNext(commandConfig);

            this.Commands.CommandExecuted += Commands_CommandExecuted;
            this.Commands.CommandErrored += Commands_CommandErrored;

            this.Commands.RegisterCommands<UserCommands>();

            await this.Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private async Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            e.Context.Client.Logger.LogError(BotEventId, $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            if (e.Exception is ChecksFailedException ex)
            {
                var emoji = DiscordEmoji.FromName(e.Context.Client, ":no_entry:");

                var embed = new DiscordEmbedBuilder
                {
                    Title = "Access denied",
                    Description = $"{emoji} You do not have the permissions required to execute this command.",
                    Color = new DiscordColor(0xFF0000) // Red
                };
                await e.Context.RespondAsync(embed);
            }
        }

        private Task Commands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
        {
            e.Context.Client.Logger.LogInformation(BotEventId, $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'");

            return Task.CompletedTask;
        }

        private Task Client_ClientErrored(DiscordClient sender, ClientErrorEventArgs e)
        {
            sender.Logger.LogError(BotEventId, e.Exception, "Exception occured");

            return Task.CompletedTask;
        }

        private Task Client_GuildAvailable(DiscordClient sender, GuildCreateEventArgs e)
        {
            sender.Logger.LogInformation(BotEventId, $"Guild available: {e.Guild.Name}");

            return Task.CompletedTask;
        }

        private Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            sender.Logger.LogInformation(BotEventId, "Client is ready.");

            return Task.CompletedTask;
        }
    }
}
