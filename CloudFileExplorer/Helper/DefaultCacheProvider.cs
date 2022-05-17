using Microsoft.Extensions.Caching.Memory;
using OnceMi.AspNetCore.OSS;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudFileExplorer.Helper
{
    public class DefaultCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache=new MemoryCache(new MemoryCacheOptions());
        public T Get<T>(string key) where T : class
        {
           return  _memoryCache.Get<T>(key);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void Set<T>(string key, T value, TimeSpan ts) where T : class
        {
            _memoryCache.Set<T>(key, value, ts);
        }
    }
}
