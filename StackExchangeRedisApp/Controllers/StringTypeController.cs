using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeRedisApp.Services;

namespace StackExchangeRedisApp.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0); //Redis 0.veritabanına bağlanıyoruz.
        }

        public IActionResult Index()
        {
            db.StringSet("name", "Ahmet Mehmet"); // Redis'e string veri ekliyoruz.
            db.StringSet("ziyaretci", 100); // Redis'e string veri ekliyoruz.

            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name"); // Redis'ten string veri alıyoruz.
            
            var value2 = db.StringGetRange("name", 0 , 4); // Redis'ten string veri alıyoruz.

            var value3 = db.StringLength("name"); // Redis'ten string verinin uzunluğunu alıyoruz.

            //db.StringIncrement("ziyaretci", 1); // Redis'teki string veriyi 1 artırıyoruz.

            //db.StringDecrementAsync("ziyaretci", 1); // Redis'teki string veriyi 1 azaltıyoruz.



            if (value2.HasValue)
            {
                ViewBag.value = value2.ToString(); // Redis'ten aldığımız string veriyi ViewBag'e atıyoruz.
            }

            return View();
        }
    }
}
