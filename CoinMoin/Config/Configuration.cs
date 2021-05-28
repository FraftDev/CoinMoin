using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoinMoin.Config
{
    class Configuration
    {
        public string Token { get; set; }
        public string Prefix { get; set; }

        public static Configuration GetInstance()
        {
            return new Configuration();
        }

        public static Configuration LoadFromFile(string Path = @"Config/Config.json")
        {
            string jsonConfig = "";
            using (var fs = File.OpenRead(Path))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                jsonConfig = sr.ReadToEnd();

            return JsonConvert.DeserializeObject<Configuration>(jsonConfig);
        }
    }
}
