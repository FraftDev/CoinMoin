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

        [Command("info")]
        public async Task GetHelp(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Green)
                .WithTitle("CoinMoin Info")
                .AddField("Supported Coins", "btc,eth,bnb,ada,doge,xrp,dot,icp,bch,uni,ltc,link,matic,xlm,sol")
                .AddField("Get Price Info", "-cm price [symbol] ([currency usd/eur])")
                .WithFooter("Made with ❤ by FraftDev");

            await ctx.Channel.SendMessageAsync(embedBuilder.Build());
        }

        [Command("price")]
        [Description("Get USD price for cryptocurrency")]
        public async Task GetPrice(CommandContext ctx, string symbol, string currency = "usd")
        {
            await ctx.TriggerTypingAsync();

            Coin coin = new Coin(symbol);

            if(coin.Id == null)
            {
                Discord​Embed​Builder embedFailBuilder = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Red)
                .WithTitle("CoinMoin Crypto Prices")
                .WithThumbnail("https://uxwing.com/wp-content/themes/uxwing/download/01-user_interface/cancel.png")
                .AddField("Invalid Input", "No Coin " + symbol)
                .AddField("List All Coins", "-cm list")
                .WithFooter("Made with ❤ by FraftDev");

                await ctx.Channel.SendMessageAsync(embedFailBuilder.Build());
                return;
            }

            bool isUsd = currency == "usd";

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
            .AddField("Price", String.Format(isUsd ? "${0:n}" : "{0:n}€", isUsd ? coin.PriceUSD : coin.PriceEUR))
            .AddField("Market Cap", String.Format(isUsd ? "${0:n}" : "{0:n}€", isUsd ? coin.MarketCapUSD : coin.MarketCapEUR))
            .AddField("Volume 24H", String.Format(isUsd ? "${0:n}" : "{0:n}€", isUsd ? coin.Volume24HUSD : coin.MarketCapEUR))
            .AddField("Change 24H", String.Format("{0:n}%", coin.Change24H))
            .AddField("Updated At", coin.UpdatedAt.ToLongTimeString())
            .WithFooter("Made with ❤ by FraftDev");

            await ctx.Channel.SendMessageAsync(embedBuilder.Build());
        }
    }
}
