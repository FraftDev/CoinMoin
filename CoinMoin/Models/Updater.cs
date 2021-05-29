using CoinMoin.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using static CoinMoin.Utility.Logger;

namespace CoinMoin.Models
{
    class Updater
    {
        public List<Coin> Coins { get; set; }
        public Database Database { get; set; }
        public Logger Logger { get; set; }

        public Updater(Database _database)
        {
            Coins = new List<Coin>();
            Database = _database;
            Logger = new Logger("UpdaterLogger", 1, Environment.CurrentDirectory + "\\Log\\UpdaterLog.txt");
        }

        public List<string> GetCoinIds()
        {
            string query = "SELECT ID FROM cm_coins";
            var coinIdList = Database.GetData(query);
            return coinIdList.AsEnumerable()
                .Select(x => x.Field<string>("ID"))
                .ToList();
        }

        public string GetUpdatedJSON()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    string url = $"https://api.coingecko.com/api/v3/simple/price?ids={string.Join("%2C", GetCoinIds())}&vs_currencies=usd%2Ceur&include_market_cap=true&include_24hr_vol=true&include_24hr_change=true&include_last_updated_at=true";
                    return webClient.DownloadString(url);
                }
                catch (WebException webException)
                {
                    Logger.Write(LogSeverity.Error, "Get Updated List Error", webException);
                    return null;
                }
            }
        }

        public List<Coin> GetUpdatedCoinList()
        {
            string updatedJSON = GetUpdatedJSON();

            if (string.IsNullOrEmpty(updatedJSON))
            {
                Logger.Write(LogSeverity.Warning, "Updater Error, JSON empty.");
                return null;
            }

            dynamic dynCoinData = JsonConvert.DeserializeObject<dynamic>(updatedJSON);

            List<Coin> coinList = new List<Coin>();

            foreach(var coinId in GetCoinIds())
            {
                Coin coin = new Coin();
                coin.InitializeFromDynamic(dynCoinData[coinId], coinId);
                coinList.Add(coin);
            }

            return coinList;
        }

        public void UpdateDatabase()
        {
            List<Coin> upToDateCoins = GetUpdatedCoinList();

            if(upToDateCoins == null)
            {
                Logger.Write(LogSeverity.Warning, "Updater Error, Coin List empty.");
                return;
            }

            foreach (var coin in upToDateCoins)
                Database.Push(coin.ToUpdateQuery());
        }
    }
}
