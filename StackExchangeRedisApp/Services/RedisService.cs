using StackExchange.Redis;

namespace StackExchangeRedisApp.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        private ConnectionMultiplexer _redis;

        public IDatabase db { get; set; }
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }

        public void Connect() //Redis'e bağlanmak için kullanılır.
        {
            var configString = $"{_redisHost}:{_redisPort}";

            _redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int db) //Redis veritabanını almak için kullanılır.
        {
            return _redis.GetDatabase(db);
        }
    }
}
