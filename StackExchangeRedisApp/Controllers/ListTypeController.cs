using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeRedisApp.Services;

namespace StackExchangeRedisApp.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        private string listKey = "names"; // Redis'te kullanacağımız anahtar.
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0); //Redis 1.veritabanına bağlanıyoruz.
        }

        public IActionResult Index()
        {
            List<string> namesList = new List<string>();

            if(db.KeyExists(listKey)) // Redis'te anahtar var mı kontrol ediyoruz.
            {
                var redisList = db.ListRange(listKey); // Redis'ten listeyi alıyoruz.
                foreach (var item in redisList)
                {
                    namesList.Add(item.ToString()); // Liste elemanlarını string'e çevirip ekliyoruz.
                }

                //Alternatif foreach yapısı
                //db.ListRange(listKey).ToList().ForEach(x =>
                //{
                //    namesList.Add(x.ToString()); // Liste elemanlarını string'e çevirip ekliyoruz.
                //});
            }

            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListLeftPush(listKey, name); // ListRightPush ile listenin sonuna veri ekliyoruz. ListLeftPush ile listenin başına eklenir.

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            db.ListRemoveAsync(listKey, name).Wait(); // Redis'ten liste elemanını siliyoruz. Geri dönüş değeriyle ilgilenmediğimiz için async metodu wait ediyoruz.

            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listKey); // ListLeftPop ile listenin başındaki elemanı siliyoruz. ListRightPop ile listenin sonundan siliyor.

            return RedirectToAction("Index");
        }
    }
}
