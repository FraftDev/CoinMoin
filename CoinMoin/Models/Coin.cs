using CoinMoin.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using static CoinMoin.Utility.Logger;

namespace CoinMoin.Models
{
    class Coin
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double PriceUSD { get; set; }
        public double PriceEUR { get; set; }
        public double MarketCapUSD { get; set; }
        public double MarketCapEUR { get; set; }
        public double Volume24HEUR { get; set; }
        public double Volume24HUSD { get; set; }
        public double Change24H { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Logger Logger { get; set; }

        public Coin()
        {
            Logger = new Logger("CoinLogger", 1, Environment.CurrentDirectory + "\\Log\\CoinLog.txt");
        }

        public string ToUpdateQuery()
        {
            return $"UPDATE cm_coins SET PRICE_USD = '{PriceUSD}', PRICE_EUR = '{PriceEUR}', MARKETCAP_USD = '{MarketCapUSD}', MARKETCAP_EUR = '{MarketCapEUR}', 24H_VOLUME_USD = '{Volume24HUSD}', 24H_VOLUME_EUR = '{Volume24HEUR}', 24H_CHANGE = '{Change24H}', UPDATED_AT = '{UpdatedAt.ToString("yyyy-MM-dd hh:mm:ss")}' WHERE ID = '{Id}'";
        }

        public void InitializeFromDynamic(dynamic dynCoin, string coinId)
        {
            try
            {
                Id = coinId;
                PriceUSD = (double)dynCoin.usd;
                PriceEUR = (double)dynCoin.eur;
                MarketCapUSD = (double)dynCoin.usd_market_cap;
                MarketCapEUR = (double)dynCoin.eur_market_cap;
                Volume24HUSD = (double)dynCoin.usd_24h_vol;
                Volume24HEUR = (double)dynCoin.eur_24h_vol;
                Change24H = (double)dynCoin.usd_24h_change;
                UpdatedAt = Helper.UnixTimeStampToDateTime((long)dynCoin.last_updated_at);
            }
            catch (Exception exception)
            {
                Logger.Write(LogSeverity.Error, "Coin Class Mapping Error", exception);
            }
        }
    }
}
