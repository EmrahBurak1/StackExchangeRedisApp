using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeRedisApp.Services;

namespace StackExchangeRedisApp.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        private string listKey = "sortedsetnames"; // Redis'te kullanacağımız anahtar.
        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0); //Redis 1.veritabanına bağlanıyoruz.
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>(); //Hashset olarak kaydedince sırası önemli değil random kaydeder.

            if(db.KeyExists(listKey))
            {
                //db.SortedSetScan(listKey).ToList().ForEach(x => //SortedSetScan rediste nasıl bir sıralama varsa o sıra ile getirir.
                //{
                //    list.Add(x.ToString()); // Liste elemanlarını string'e çevirip ekliyoruz.
                //});

                db.SortedSetRangeByRank(listKey,0,5, order:Order.Descending).ToList().ForEach(x => //SortedSetRangeByRank ile sıralama yapıyoruz. Index değerleri ile yani 0 dan 5 e kadar olan değerleri al diyebiliyoruz. Descending ile büyükten küçüğe sıralayabiliyoruz.
                {
                    list.Add(x.ToString()); // Liste elemanlarını string'e çevirip ekliyoruz.
                });
            }
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1)); // Redis'te anahtarın süresini 1 dakika olarak ayarlıyoruz.

            db.SortedSetAdd(listKey, name, score); //SortedSetAdd ile listeye veri eklenir. Score ile de sıralaması verilebilir.

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            db.SortedSetRemove(listKey, name); // Redis'ten liste elemanını siliyoruz.

            return RedirectToAction("Index");
        }
    }
}
