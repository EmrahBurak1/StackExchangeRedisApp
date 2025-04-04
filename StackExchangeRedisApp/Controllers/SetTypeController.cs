using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeRedisApp.Services;

namespace StackExchangeRedisApp.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        private string listKey = "hashnames"; // Redis'te kullanacağımız anahtar.
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0); //Redis 1.veritabanına bağlanıyoruz.
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>(); //Hashset olarak kaydedince sırası önemli değil random kaydeder.

            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString()); // Liste elemanlarını string'e çevirip ekliyoruz.
                });
            }

            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            //if(!db.KeyExists(listKey)) // Redis'te anahtar var mı kontrol ediyoruz. İlgili anahtar yoksa süreyi 5 dk ayarlıyoruz. Bu kontrolü yapmazsak Add methoduna her girdiğinde 5 dk yenilenir.
            //{
            //    db.KeyExpire(listKey, DateTime.Now.AddMinutes(5)); // Redis'te anahtarın süresini 5 dakika olarak ayarlıyoruz.
            //}

            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5)); // Redis'te anahtarın süresini 5 dakika olarak ayarlıyoruz.

            db.SetAdd(listKey, name); //SetAdd ile listeye veri eklenir.


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            await db.SetRemoveAsync(listKey, name); // Redis'ten liste elemanını siliyoruz.
            return RedirectToAction("Index");
        }
    }
}
