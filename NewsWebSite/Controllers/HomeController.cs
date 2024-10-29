using Application.HomePageService;
using Application.HomePageService.Dto;
using Infrastructures.CacheHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NewsWebSite.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace NewsWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomePageService homePageService;
        private readonly IDistributedCache cache;

        public HomeController(IHomePageService homePageService, IDistributedCache cache)
        {
            this.homePageService = homePageService;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            HomePageDto homePageData = new HomePageDto();

            // Attempt to retrieve cached data using a generated cache key
            var homePageCache = cache.GetAsync(CacheHelper.GenerateHomePageCacheKey()).Result;

            if (homePageCache != null)
            {
                // Deserialize the cached JSON data back into a HomePageDto object
                homePageData = JsonSerializer.Deserialize<HomePageDto>(homePageCache);
            }
            else
            {
                homePageData = homePageService.GetData();

                // Serialize the HomePageDto object into a JSON string
                string jsonData = JsonSerializer.Serialize(homePageData);

                // Convert the JSON string into a byte array using UTF-8 encoding
                byte[] encodedJson = Encoding.UTF8.GetBytes(jsonData);

                // Set cache options, including a sliding expiration time
                var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(CacheHelper.DefaultCacheDuration);

                // Store the newly fetched data in the cache for future requests
                cache.SetAsync(CacheHelper.GenerateHomePageCacheKey(), encodedJson, options);
            }
            return View(homePageData);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}