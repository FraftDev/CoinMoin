using System;
using System.Collections.Generic;
using System.Text;

namespace CoinMoin.Config
{
    public interface IConfig<T>
    {
        IConfig<T> LoadFromFile();
    }
}
