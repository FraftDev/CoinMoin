using System;
using System.Collections.Generic;
using System.Text;

namespace CoinMoin.Utility
{
    class Helper
    {
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).DateTime;
        }
    }
}
