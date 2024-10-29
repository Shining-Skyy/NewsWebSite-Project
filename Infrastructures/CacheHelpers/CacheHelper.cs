using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.CacheHelpers
{
    public static class CacheHelper
    {
        public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(5);

        public static string GenerateHomePageCacheKey()
        {
            return "HomePage";
        }
    }
}
