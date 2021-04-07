using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common;

namespace Web.NETCoreAPI.Controllers
{
    /// <summary>
    /// 缓存
    /// </summary>
    [Route("api/[controller]")]
    public class CacheController : Controller
    {

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <returns></returns>
        [Route("RemoveCacheAll")]
        [HttpPost]
        public void RemoveCacheAll()
        {
            MemoryCacheHelper.RemoveCacheAll();
        }

        /// <summary>
        ///  清除匹配到的缓存
        /// </summary>
        /// <param name="pattern"></param>
        [Route("RemoveCacheRegex")]
        [HttpPost]
        public void RemoveCacheRegex(string pattern)
        {
            MemoryCacheHelper.RemoveCacheRegex(pattern);
        }

        /// <summary>
        /// 获取所有缓存类
        /// </summary>
        /// <returns></returns>
        [Route("GetCacheKeys")]
        [HttpPost]
        public List<string> GetCacheKeys()
        {
            return MemoryCacheHelper.GetCacheKeys();
        }

    }
}
