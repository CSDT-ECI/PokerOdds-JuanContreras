﻿using System;
using System.IO;
using System.Runtime.Caching;
//using Microsoft.WindowsAzure.ServiceRuntime;
using Owin;
using PokerOdds.Web.OWIN.Routers;
using System.Linq;
using PokerOdds.HoldemOdds;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;
using PokerOdds.Web.OWIN.Cache;

namespace PokerOdds.Web.OWIN
{
    public class Startup
    {
        private FileBackedMemoryCache _cache = null;

        public void Configuration(IAppBuilder app)
        {
            //var cachePath = RoleEnvironment.IsAvailable ?
            //    RoleEnvironment.GetLocalResource("fileCache").RootPath :
            //    Path.Combine(Path.GetTempPath(), "PokerOdds.Web.OWIN");

            //Directory.CreateDirectory(cachePath);

            //_cache = new FileBackedMemoryCache("cache", cachePath);

            //_cache.MaxCacheSize = RoleEnvironment.IsAvailable ?
            //    RoleEnvironment.GetLocalResource("fileCache").MaximumSizeInMegabytes * 1024L * 1024L :
            //    2048L * 1024L * 1024L; //2GB

            //_cache.MaxCacheSizeReached += MaxCacheSizeReached;

            //PrimeCache();

            var router = new RequestRouter { Cache = _cache };

            app.Run(router.HandleRequest);
        }

        private void MaxCacheSizeReached(object sender, FileCacheEventArgs e)
        {
            //first try removing all non-completed items
            foreach (var odds in _cache.GetKeys()
                        .Select(key => _cache.GetCacheItem(key))
                        .Select(item => item.Value)
                        .OfType<TexasHoldemOdds>()
                        .Where(odds => !odds.Completed))
            {
                try
                {
                    _cache.Remove(odds.GetCacheKey());
                }
                catch { }
            }

            //if that has bought us some space
            if (_cache.GetCacheSize() < e.CurrentCacheSize)
            {
                //bail
                return;
            }

            //final last ditch approach
            //remove everything, and re-prime the cache
            _cache.Flush();
            PrimeCache();
        }

        private void PrimeCache()
        {
            lock (_cache)
            {
                var policy = new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable };

                TexasHoldemOdds[] oddsList;

                using (var cacheFile = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("PokerOdds.Web.OWIN.Cache.PrimeCache.json")))
                {
                    oddsList = JsonConvert.DeserializeObject<TexasHoldemOdds[]>(cacheFile.ReadToEnd());
                }

                foreach (var odds in oddsList)
                {
                    var keys = new string[0];

                    try
                    {
                        keys = _cache.GetKeys();
                    }
                    catch { }

                    if (!keys.Contains(odds.GetCacheKey()))
                    {
                        _cache.Add(odds.GetCacheKey(), odds, policy);
                    }
                }
            }
        }
    }
}