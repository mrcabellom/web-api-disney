using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.RedisCache
{
    public sealed class NoCache
        : ICache
    {
        public bool Exists(string key)
        {
            return false;
        }

        public void FlushCache()
        {
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public T GetOrInsert<T>(string key, Func<T> resultFunc, TimeSpan? caducidad = default(TimeSpan?))
        {
            return default(T);
        }

        public void Insert<T>(string key, T valor, TimeSpan? caducidad = default(TimeSpan?))
        {
        }

        public long Remove(IEnumerable<string> keys)
        {
            return 0;
        }

        public bool Remove(string key)
        {
            return false;
        }
    }
}
