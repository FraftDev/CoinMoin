using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoinMoin.Config
{
    class BotConfiguration
    {
        public string Token { get; set; }
        public string Prefix { get; set; }

        public static BotConfiguration GetInstance()
        {
            return new BotConfiguration();
        }

        public static BotConfiguration LoadFromFile(string Path = @"Config/BotConfig.json")
        {
            string jsonConfig = "";
            using (var fs = File.OpenRead(Path))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                jsonConfig = sr.ReadToEnd();

            return JsonConvert.DeserializeObject<BotConfiguration>(jsonConfig);
        }
    }
}
