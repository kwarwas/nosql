using System;
using System.Threading.Tasks;
using EventSourcing.Common.Events;
using EventSourcing.Common.Helpers;
using EventStore.ClientAPI;

namespace EventSourcing.Subscriber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = ConnectionSettings.Create()
                .UseConsoleLogger();

            var conn = EventStoreConnection.Create("ConnectTo=tcp://admin:changeit@localhost:1113", builder);
            
            await conn.ConnectAsync();
            
            const string streamName = "e-commerce-stream";
            const string groupName = "start-from-current";

            var settings = PersistentSubscriptionSettings.Create()
                .DoNotResolveLinkTos()
                .StartFromCurrent();
            
            try
            {
                await conn.CreatePersistentSubscriptionAsync(streamName, groupName, settings, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            await conn.ConnectToPersistentSubscriptionAsync(streamName, groupName, EventAppeared);
            
            Console.WriteLine("Waiting for events. Press a key to quit.");
            Console.ReadLine();
        }

        private static void EventAppeared(EventStorePersistentSubscriptionBase eventStorePersistentSubscriptionBase, ResolvedEvent @event)
        {
            var data = JsonHelper.Deserialize<OrderSubmitted>(@event.Event.Data);
            Console.WriteLine(data);
        }
    }
}