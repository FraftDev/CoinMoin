using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoinMoin.Config
{
    public partial class DatabaseConfig : IConfig<DatabaseConfig>
    {
        [JsonProperty("DB_HOST")]
        public string Host { get; set; }

        [JsonProperty("DB_NAME")]
        public string Name { get; set; }
        
        [JsonProperty("DB_USER")]
        public string User { get; set; }
        [JsonProperty("DB_PASS")]
        public string Pass { get; set; }

        public static DatabaseConfig GetInstance()
        {
            return new DatabaseConfig();
        }

        public static DatabaseConfig LoadFromFile(string Path = @"Config/DatabaseConfig.json")
        {
            string jsonConfig = "";
            using (var fs = File.OpenRead(Path))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                jsonConfig = sr.ReadToEnd();

            return JsonConvert.DeserializeObject<DatabaseConfig>(jsonConfig);
        }

        IConfig<DatabaseConfig> IConfig<DatabaseConfig>.LoadFromFile() => LoadFromFile();
    }
}
