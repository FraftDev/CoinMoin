using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoinMoin.Commands
{
    class UserCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Test if the Bot is responsive")] 
        [Aliases("pong")]
        public async Task Ping(CommandContext ctx) 
        {
            await ctx.TriggerTypingAsync();

            var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

            await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
        }

        [Command("price")]
        [Description("Get USD price for cryptocurrency")]
        public async Task GetPrice(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();


        }
    }
}
