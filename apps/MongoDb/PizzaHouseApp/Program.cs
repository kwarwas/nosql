using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using PizzaHouseApp.Database;
using PizzaHouseApp.Model;

namespace PizzaHouseApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new DatabaseContext("pizzaHouse");

            EntityMappings.Map();

            var collection = context.GetCollection<PizzaHouseItem>("Menu");

            await Query(collection);

            await Insert(collection);
        }
        
        private static Task Query(IMongoCollection<PizzaHouseItem> collection)
        {
            Console.WriteLine("-- PIZZA --");
            
            var pizzas = collection.AsQueryable().OfType<Pizza>();

            foreach (var pizza in pizzas)
            {
                Console.WriteLine(pizza);
            }

            Console.WriteLine("-- PASTA --");
            
            var pastas = collection.AsQueryable().OfType<Pasta>();

            foreach (var pasta in pastas)
            {
                Console.WriteLine(pasta);
            }

            Console.WriteLine("-- DRINK --");
            
            var drinks = collection.AsQueryable().OfType<Drink>();

            foreach (var drink in drinks)
            {
                Console.WriteLine(drink);
            }

            return Task.CompletedTask;
        }
        
        private static async Task Insert(IMongoCollection<PizzaHouseItem> collection)
        {
            var cola = new Drink
            {
                Id = 15,
                Name = "Cola",
                Size = 0.3,
                Kind = new [] { "Coca‑Cola", "Diet Coke", "Coke Zero Sugar", "Coca‑Cola Life", "Cherry Coke" },
                Price = 4
            };

            await collection.InsertOneAsync(cola);
        }
    }
}