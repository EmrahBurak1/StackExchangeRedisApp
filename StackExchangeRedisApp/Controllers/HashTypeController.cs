using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeRedisApp.Services;

namespace StackExchangeRedisApp.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        public string hashKey { get; set; } = "sozluk";
        public HashTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0); //Redis 1.veritabanına bağlanıyoruz.
        }
        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>(); //Hash tipinde bir sözlük oluşturuyoruz. Key ve value olarak değerleri tutar. Her bir değer'in bir key'i tanımlanmalıdır. Distionary hızlıca veriye erişmek için kullanılır.

            if(db.KeyExists(hashKey)) //Eğer hashKey varsa
            {
                //db.HashGet(hashKey, "pen"); //Bu şekilde tüm datayı almak yerine tek bir datayı alabiliriz.
                db.HashGetAll(hashKey).ToList().ForEach(x => //Hash'teki tüm verileri alıyoruz.
                {
                    list.Add(x.Name.ToString(), x.Value.ToString()); //Verileri sözlüğe ekliyoruz.
                });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            db.HashSet(hashKey, name, value); // HashSet ile hash'e veri ekliyoruz. Anahtar ve değer aynı olduğu için iki kez aynı veriyi ekliyoruz.
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            db.HashDelete(hashKey, name); //Hash'ten veri silmek için kullanılır.
            return RedirectToAction("Index");
        }
    }
}
