using StackExchange.Redis;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace Cache.General
{
    public class RedisCache<T> : ICache<T>
    {
        private readonly ConnectionMultiplexer redisConnection;
        private readonly string prefix;

        private readonly DataContractSerializer serializer = new DataContractSerializer(typeof(T));

        public RedisCache(string hostName, string prefix)
        {
            this.prefix = prefix;
            var options = new ConfigurationOptions()
            {
                AbortOnConnectFail = false,
                EndPoints = { hostName }
            };
            redisConnection = ConnectionMultiplexer.Connect(options);
        }

        public T Get(string key)
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(prefix + key);
            if (s == null)
            {
                return default(T);
            }

            return (T)serializer.ReadObject(new MemoryStream(s));
        }

        public void Set(string key,T value, DateTimeOffset expirationDate)
        {
            var db = redisConnection.GetDatabase();
            var redisKey = prefix + key;
 
            if (value == null)
            {
                db.StringSet(redisKey, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, value);
                db.StringSet(redisKey, stream.ToArray(), expirationDate - DateTimeOffset.Now);
            }
        }
    }
}
