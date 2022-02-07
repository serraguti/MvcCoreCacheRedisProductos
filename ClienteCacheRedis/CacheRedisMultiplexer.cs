using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClienteCacheRedis
{
    public static class CacheRedisMultiplexer
    {
        private static Lazy<ConnectionMultiplexer> CreateConnection =
            new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("cacheredisproductospgs.redis.cache.windows.net:6380,password=zjFtjCKrqLE7KAVsSkSqobPi5QN15Tu2tAzCaEFnbZ8=,ssl=True,abortConnect=False");
            });

        public static ConnectionMultiplexer GetConnection
        {
            get
            {
                return CreateConnection.Value;
            }
        }
    }
}
