using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "sortedsetnames";
        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(3);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                //db.SortedSetScan(listKey).ToList().ForEach(x =>
                //{
                //    list.Add(x.ToString());
                //});
                db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            db.SortedSetAdd(listKey, name, score);
            return RedirectToAction("Index");

        }
        public IActionResult DeleteItem(string name)
        {
            db.SortedSetRemove(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
