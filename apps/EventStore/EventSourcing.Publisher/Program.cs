using System;
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
                .UseConsoleLogger();

            var conn = EventStoreConnection.Create("ConnectTo=tcp://admin:changeit@localhost:1113", builder);

            await conn.ConnectAsync();

            const string streamName = "e-commerce-stream";

            for (int i = 0; i < 10; i++)
            {
                var orderSubmitted = new OrderSubmitted
                (
                    Guid.NewGuid(),
                    $"Order {DateTime.Now} - {i + 1}",
                    (decimal) Rnd.NextDouble() * 100,
                     (OrderCategory)Rnd.Next(Enum.GetValues(typeof(OrderCategory)).Length)
                );

                var data = JsonHelper.Serialize(orderSubmitted);
                var meta = JsonHelper.Serialize(new {Index = i});

                var eventData = new EventData(Guid.NewGuid(), typeof(OrderSubmitted).Name, true, data, meta);
                await conn.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventData);
            }
        }
    }
}