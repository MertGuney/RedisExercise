using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            //_distributedCache.SetString("name", "guneymert", cacheEntryOptions);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 100 };
            string jsonProduct = JsonConvert.SerializeObject(product);
            //_distributedCache.SetString("product:1", jsonProduct, cacheEntryOptions);
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            _distributedCache.Set("product:1", byteProduct);


            return View();
        }
        public IActionResult Show()
        {
            //string name = _distributedCache.GetString("name");
            //ViewBag.name = name;

            //string jsonProduct = _distributedCache.GetString("product:1");


            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.product = p;

            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/a.jpg");
            Byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("image", imageByte);
            return View();
        }
        public IActionResult ImageUrl()
        {
            Byte[] imageByte = _distributedCache.Get("image");


            return File(imageByte, "image/jpg");
        }
    }
}
