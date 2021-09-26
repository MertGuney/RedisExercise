using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            db.StringSet("name", "Mert Guney");
            db.StringSet("ziyaretci", 100);
            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");
            db.StringIncrement("ziyaretci", 2);
            //async await ikilisini kullanmadan direkt sonuç olmak istiyorsak her ikisinide kullanabiliriz.
            //db.StringDecrementAsync("ziyaretci", 10).Wait();
            var count = db.StringDecrementAsync("ziyaretci", 1).Result;
            if (!value.IsNullOrEmpty)
            {
                ViewBag.value = value.ToString();
            }
            return View();
        }
    }
}
