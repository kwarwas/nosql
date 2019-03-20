using System;
using System.Threading.Tasks;
using Neo4jClient;
using Newtonsoft.Json;

namespace Neo4jApp
{
    class Player
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("score")] public int Score { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Score: {Score}";
        }
    }

    class PlayedRelationship
    {
        [JsonProperty("won")] public bool Won { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var uri = "bolt://localhost";
            var user = "neo4j";
            var password = "password";

            using (var client = new BoltGraphClient(uri, user, password))
            {
                await client.ConnectAsync();

                await Query1(client);

                await Query2(client);

                await Create1(client);

                await Set1(client);

                await Delete1(client);
            }
        }

        private static async Task Query1(ICypherGraphClient client)
        {
            Console.WriteLine("Simple node query");

            var players = await client.Cypher
                .Match("(x:Player)")
                .Where<Player>(x => x.Name == "Adam" || x.Name == "Jola")
                .Return(x => x.As<Player>())
                .ResultsAsync;

            foreach (var player in players)
            {
                Console.WriteLine(player);
            }
        }

        private static async Task Query2(ICypherGraphClient client)
        {
            Console.WriteLine("Simple pattern query");

            var query = client.Cypher
                .Match("(x:Player)-[y:PLAYED]->(z:Player)")
                .Where<Player, PlayedRelationship>((x, y) => x.Score > 90 && y.Won)
                .Return((x, z) => new
                {
                    Winner = x.As<Player>(),
                    Looser = z.As<Player>()
                })
                .OrderByDescending("y.date");

            Console.WriteLine(query.Query.QueryText);

            var results = await query.ResultsAsync;

            foreach (var item in results)
            {
                Console.WriteLine("{0} won with {1}", item.Winner.Name, item.Looser.Name);
            }
        }

        private static async Task Create1(ICypherGraphClient client)
        {
            Console.WriteLine("Create example");

            var query = client.Cypher
                .Match("(x:Player)")
                .Where<Player>(x => x.Name == "Jan")
                .Create("(x)<-[:PLAYED {relData}]-(y:Player {playerData})")
                .WithParam("playerData", new 
                {
                    name = "Karol", score = 120
                })
                .WithParam("relData", new
                {
                    won = true, date = DateTime.Now
                });

            Console.WriteLine(query.Query.QueryText);

            await query.ExecuteWithoutResultsAsync();
        }

        private static async Task Set1(ICypherGraphClient client)
        {
            Console.WriteLine("Set example");

            var query = client.Cypher
                .Match("(x:Player)")
                .Where<Player>(x => x.Name == "Jan")
                .Set("x.name = $name")
                .WithParam("name", "John");

            Console.WriteLine(query.Query.QueryText);

            await query.ExecuteWithoutResultsAsync();
        }
        
        private static async Task Delete1(ICypherGraphClient client)
        {
            Console.WriteLine("Delete example");

            var query = client.Cypher
                .Match("(x:Player)")
                .Where<Player>(x => x.Name == "Karol")
                .DetachDelete("x");

            Console.WriteLine(query.Query.QueryText);

            await query.ExecuteWithoutResultsAsync();
        }
    }
}