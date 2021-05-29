using CoinMoin.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public async Task GetPrice(CommandContext ctx, string symbol, string currency = "usd")
        {
            await ctx.TriggerTypingAsync();

            Coin coin = new Coin(symbol);

            if(coin.Id == null)
            {
                Discord​Embed​Builder embedBuilder = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Red)
                .WithTitle("CoinMoin Crypto Prices")
                .WithThumbnail("https://uxwing.com/wp-content/themes/uxwing/download/01-user_interface/cancel.png")
                .AddField("Invalid Input", "No Coin " + symbol)
                .AddField("List All Coins", "-cm list")
                .WithFooter("Made with ❤ by FraftDev");

                await ctx.Channel.SendMessageAsync(embedBuilder.Build());
                return;
            }

            if(currency == "usd")
            {
                DiscordColor discordColor;
                if (coin.Change24H > 0)
                    discordColor = DiscordColor.Green;
                else
                    discordColor = DiscordColor.Red;

                Discord​Embed​Builder embedBuilder = new DiscordEmbedBuilder()
                .WithColor(discordColor)
                .WithTitle("CoinMoin Crypto Prices")
                .WithThumbnail(coin.GetThumbnail())
                .AddField("Name", coin.Name)
                .AddField("Price", String.Format("${0:n}", coin.PriceUSD))
                .AddField("Market Cap", String.Format("${0:n}", coin.MarketCapUSD))
                .AddField("Volume 24H", String.Format("${0:n}", coin.Volume24HUSD))
                .AddField("Change 24H", String.Format("{0:n}%", coin.Change24H))
                .AddField("Updated At", coin.UpdatedAt.ToLongTimeString())
                .WithFooter("Made with ❤ by FraftDev");

                await ctx.Channel.SendMessageAsync(embedBuilder.Build());
            }

            if(currency == "eur")
            {
                DiscordColor discordColor;
                if (coin.Change24H > 0)
                    discordColor = DiscordColor.Green;
                else
                    discordColor = DiscordColor.Red;

                Discord​Embed​Builder embedBuilder = new DiscordEmbedBuilder()
                .WithColor(discordColor)
                .WithTitle("CoinMoin Crypto Prices")
                .WithThumbnail(coin.GetThumbnail())
                .AddField("Name", coin.Name)
                .AddField("Price", String.Format("{0:n}€", coin.PriceEUR))
                .AddField("Market Cap", String.Format("{0:n}€", coin.MarketCapEUR))
                .AddField("Volume 24H", String.Format("{0:n}€", coin.Volume24HEUR))
                .AddField("Change 24H", String.Format("{0:n}%", coin.Change24H))
                .AddField("Updated At", coin.UpdatedAt.ToLongTimeString())
                .WithFooter("Made with ❤ by FraftDev");

                await ctx.Channel.SendMessageAsync(embedBuilder.Build());
            }
        }
    }
}
