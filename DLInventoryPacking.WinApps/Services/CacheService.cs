using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLInventoryPacking.WinApps.Services
{
    public static class CacheService
    {
        private static readonly Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            var cacheConnection = "dl-cache.redis.cache.windows.net:6380,password=fspdi3JK1OY3uFXzl1XyB3igmqSKwnSq4IkHs3pKVe0=,ssl=True,abortConnect=False";
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return _lazyConnection.Value;
            }
        }
    }
}
