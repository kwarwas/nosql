using System;
using System.Linq;
using System.Threading.Tasks;
using EventSourcing.Common.Events;
using EventSourcing.Common.Helpers;
using EventStore.ClientAPI;

namespace EventSourcing.Publisher
{
    class Program
    {
        private static readonly Random Rnd = new Random();

        static async Task Main(string[] args)
        {
            var builder = ConnectionSettings.Create()
                .EnableVerboseLogging()
                .UseConsoleLogger();

            var connection = EventStoreConnection.Create("ConnectTo=tcp://admin:changeit@localhost:1113", builder);

            await connection.ConnectAsync();

            const string streamName = "e-commerce-stream";

            var orders = Enumerable
                .Range(1, 10)
                .Select(i => new OrderSubmitted
                (
                    Guid.NewGuid(),
                    $"Order {DateTime.Now} - {i + 1}",
                    (decimal) Rnd.NextDouble() * 100,
                    (OrderCategory) Rnd.Next(Enum.GetValues(typeof(OrderCategory)).Length)
                ))
                .ToList();

            for (int i = 0; i < 10; i++)
            {
                var data = JsonHelper.Serialize(orders[i]);
                var meta = JsonHelper.Serialize(new {Index = i});

                var eventData = new EventData(Guid.NewGuid(), nameof(OrderSubmitted), true, data, meta);
                await connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventData);
            }

            var ordersToChange = orders
                .OrderBy(_ => Guid.NewGuid())
                .Take(5);

            foreach (var order in ordersToChange)
            {
                var priceChanged = new PriceChanged
                (
                    order.Id,
                    order.Price,
                    (decimal) Rnd.NextDouble() * 100,
                    DateTime.UtcNow
                );
                
                var data = JsonHelper.Serialize(priceChanged);

                var eventData = new EventData(Guid.NewGuid(), nameof(PriceChanged), true, data, null);
                await connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventData);
            }
        }
    }
}