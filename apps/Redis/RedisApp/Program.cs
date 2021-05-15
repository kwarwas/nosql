using System;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisApp
{
    class Product
    {
        public int Id { get; }
        public string Name { get; }
        public DateTime CreatedOn { get; }

        public Product(int id, string name, DateTime createdOn)
        {
            Id = id;
            Name = name;
            CreatedOn = createdOn;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, CreatedOn: {CreatedOn}";
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            using var redis = await ConnectionMultiplexer.ConnectAsync("localhost");
            var db = redis.GetDatabase();

            await SimpleTypes(db);

            await ComplexTypes(db);
            
            await KeyMethods(db);
            
            await Geo(db);
            
            await PubSub(redis);
        }
        
        private static async Task SimpleTypes(IDatabase db)
        {
            Console.WriteLine("-- Store and get simple values --");
            
            await db.StringSetAsync("key1", "Hello NoSQL");

            var key1Value = await db.StringGetAsync("key1");

            Console.WriteLine(" - {0} {1}", nameof(key1Value), key1Value);

            await db.StringSetAsync("key2", 2, TimeSpan.FromSeconds(2));

            var key2Value = (int) await db.StringGetAsync("key2");

            Console.WriteLine(" - {0} {1}", nameof(key2Value), key2Value);

            await Task.Delay(TimeSpan.FromSeconds(3));

            key2Value = (int) await db.StringGetAsync("key2");

            Console.WriteLine(" - {0} {1}", nameof(key2Value), key2Value);
        }

        private static async Task ComplexTypes(IDatabase db)
        {
            Console.WriteLine("-- Store and get complex values --");

            var data = JsonConvert.SerializeObject(new Product(1, "Banana", DateTime.Now));

            await db.StringSetAsync("key1", data);

            var product = JsonConvert.DeserializeObject<Product>(await db.StringGetAsync("key1"));

            Console.WriteLine(" - {0} {1}", nameof(product), product);
        }
        
        private static async Task KeyMethods(IDatabase db)
        {
            Console.WriteLine("-- Key operations --");

            db.KeyDelete("key1");

            await db.StringSetAsync("key1", "Hello NoSQL", TimeSpan.FromSeconds(3));

            Console.WriteLine(await db.KeyExistsAsync("key1"));

            await Task.Delay(TimeSpan.FromSeconds(2));

            Console.WriteLine(await db.KeyIdleTimeAsync("key1"));

            Console.WriteLine(await db.KeyTimeToLiveAsync("key1"));

            Console.WriteLine(await db.KeyPersistAsync("key1"));

            Console.WriteLine((await db.KeyTimeToLiveAsync("key1"))?.ToString() ?? "brak");

            Console.WriteLine(await db.KeyExpireAsync("key1", DateTime.Now.AddSeconds(4)));

            Console.WriteLine(await db.KeyTimeToLiveAsync("key1"));
        }
        
        private static async Task Geo(IDatabase db)
        {
            Console.WriteLine("-- GEO --");

            var geoEntries = new[]
            {
                new GeoEntry(21.03075, 52.21943, "Centrum Sztuki Współczesnej Zamek Ujazdowski"),
                new GeoEntry(21.04062, 52.22046, "Stadion Miejski Legii Warszawa im. Marszałka J. Piłsudskiego"),
                new GeoEntry(21.03747, 52.21561, "Muzeum Łazienki Królewskie"),
                new GeoEntry(21.02316, 52.21781, "Mauzoleum Walki i Męczeństwa"),
                new GeoEntry(21.02811, 52.21469, "Pomnik Fryderyka Chopina"),
                new GeoEntry(21.02764, 52.21314, "Pałac Belwederski"),
                new GeoEntry(21.02656, 52.21054, "Ambasada Federacji Rosyjskiej"),
                new GeoEntry(21.05609, 52.21106, "Kopiec Powstania Warszawskiego"),
                new GeoEntry(21.02568, 52.22204, "Park Ujazdowski"),
                new GeoEntry(21.03291, 52.21413, "I'm here")
            };

            await db.KeyDeleteAsync("places");

            await db.GeoAddAsync("places", geoEntries);

            Console.WriteLine(await db.GeoDistanceAsync("places",
                "Centrum Sztuki Współczesnej Zamek Ujazdowski",
                "Park Ujazdowski")
            );

            var places = db.GeoRadius("places", "I'm here", 1000)
                .OrderBy(x => x.Distance);

            foreach (var item in places)
            {
                Console.WriteLine("{0} {1}", item, item.Distance);
            }
        }
        
        private static async Task PubSub(IConnectionMultiplexer redis)
        {
            Console.WriteLine("-- PUB/SUB --");

            var subscriber1 = redis.GetSubscriber();

            await subscriber1.SubscribeAsync("message", Subscribe);

            var subscriber2 = redis.GetSubscriber();

            for (var i = 0; i < 5; i++)
            {
                await subscriber2.PublishAsync("message", $"Hello NoSQL {i + 1}");
            }
        }

        private static void Subscribe(RedisChannel channel, RedisValue value)
        {
            Console.WriteLine(value);
        }
    }
}