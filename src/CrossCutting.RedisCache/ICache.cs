using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.RedisCache
{
    public interface ICache
    {
        T Get<T>(string key);
        T GetOrInsert<T>(string key, Func<T> resultFunc, TimeSpan? caducidad = null);
        void Insert<T>(string key, T valor, TimeSpan? caducidad = null);
        bool Exists(string key);
        bool Remove(string key);
        long Remove(IEnumerable<string> keys);
        void FlushCache();
    }
}
