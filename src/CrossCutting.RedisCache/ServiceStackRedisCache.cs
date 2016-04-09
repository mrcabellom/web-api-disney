using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ServiceStack;
using ServiceStack.Redis;
using System.Configuration;


namespace CrossCutting.RedisCache
{
    public class ServiceStackRedisCache : ICache
    {

        private static string _redisConnectionString = string.Empty;
        private readonly JsonSerializerSettings _serializerSettings;

        public ServiceStackRedisCache()
        {
            _redisConnectionString = ConfigurationManager.AppSettings["CacheConnectionString"];

            _serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }

        public T Get<T>(string clave)
        {
            using (var manager = new RedisManagerPool(_redisConnectionString))
            {
                using (var redis = manager.GetClient())
                {
                    var dataCache = redis.GetValue(clave);
                    if (!dataCache.IsNullOrEmpty())
                    {
                        return JsonConvert.DeserializeObject<T>(dataCache, _serializerSettings);
                    }
                    return default(T);
                }
            }
        }

        public T GetOrInsert<T>(string key, Func<T> resultFunc, TimeSpan? caducidad = null)
        {
            var value = Get<T>(key);

            if (!Equals(default(T), value))
                return value;

            value = resultFunc();

            if (Equals(default(T), value))
                return value;

            Insert<T>(key, value, caducidad);
            return value;
        }

        public void Insert<T>(string clave, T valor, TimeSpan? caducidad = null)
        {
            using (var manager = new RedisManagerPool(_redisConnectionString))
            {
                using (var redis = manager.GetClient())
                {
                    var valSerializado = JsonConvert.SerializeObject(valor, _serializerSettings);
                    if (caducidad.HasValue)
                    {
                        redis.SetValue(clave, valSerializado, caducidad.Value);
                    }
                    else
                    {
                        redis.SetValue(clave, valSerializado);
                    }
                }
            }
        }

        public bool Exists(string key)
        {
            using (var manager = new RedisManagerPool(_redisConnectionString))
            {
                using (var redis = manager.GetClient())
                {
                    return redis.ContainsKey(key);
                }
            }
        }

        public bool Remove(string key)
        {
            using (var manager = new RedisManagerPool(_redisConnectionString))
            {
                using (var redis = manager.GetClient())
                {
                    return redis.Remove(key);
                }
            }
        }

        public long Remove(IEnumerable<string> keys)
        {
            using (var manager = new RedisManagerPool(_redisConnectionString))
            {
                using (var redis = manager.GetClient())
                {
                    if (keys == null || !keys.Any()) return 0;

                    redis.RemoveAll(keys);
                }
            }
            return keys.Count();
        }

        public void FlushCache()
        {
            using (var manager = new RedisManagerPool(_redisConnectionString))
            {
                using (var redis = manager.GetClient())
                {
                    redis.FlushAll();
                }
            }
        }
    }
}

