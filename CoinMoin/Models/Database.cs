using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CoinMoin.Config;
using CoinMoin.Utility;
using MySql.Data;
using MySql.Data.MySqlClient;
using static CoinMoin.Utility.Logger;

namespace CoinMoin.Models
{
    class Database
    {
        public MySqlConnection Connection { get; set; }
        public DatabaseConfig Config { get; set; }
        public Logger Logger { get; set; }

        public Database(DatabaseConfig _config)
        {
            Config = _config;
            Connection = new MySqlConnection($"Server={Config.Host};Database={Config.Name};Uid={Config.User};");
            Logger = new Logger("DatabaseLogger", 1, Environment.CurrentDirectory + "\\Log\\DatabaseLog.txt");
        }

        public DataTable GetData(string query)
        {
            using (Connection)
            {
                Connection.Open();
                using(var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    using(var dataAdapter = new MySqlDataAdapter(cmd))
                    {
                        try
                        {
                            DataTable dataTable = new DataTable();
                            dataAdapter.Fill(dataTable);
                            return dataTable;
                        }
                        catch(MySqlException mySqlException)
                        {
                            Logger.Write(LogSeverity.Error, "Get Data Exception", mySqlException);
                            return null;
                        }
                    }
                }
            }
        }

        public void Push(string query)
        {
            using (Connection)
            {
                Connection.Open();
                using(var cmd = Connection.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }
                    catch(MySqlException mySqlException)
                    {
                        Logger.Write(LogSeverity.Error, "Get Data Exception", mySqlException);
                    }
                }
            }
        }
    }
}
