using System;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TransactionApp.Entities;

namespace TransactionApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;

            var client = new MongoClient("mongodb://localhost:27017");
            
            BsonClassMap.RegisterClassMap<Blog>(x => 
            {
                x.AutoMap();
                x.MapIdMember(y => y.Id);
            });

            BsonClassMap.RegisterClassMap<Author>(x => 
            {
                x.AutoMap();
                x.MapIdMember(y => y.Id);
            });
            
            await client.DropDatabaseAsync("Blog");
            
            var authorId = Guid.Parse("c3d7d25a-31cb-4c54-99e0-833a703f4a3c");
            
            try
            {
                var author = new Author(authorId, "Alvin", "Miller");

                await client
                    .GetDatabase("Blog")
                    .GetCollection<Author>("authors")
                    .InsertOneAsync(author);

                var blog = new Blog(Guid.NewGuid(), "Example blog", authorId);

                await client
                    .GetDatabase("Blog")
                    .GetCollection<Blog>("blogs")
                    .InsertOneAsync(blog);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            using (var session = client.StartSession())
            {
                session.StartTransaction();

                await client
                    .GetDatabase("Blog")
                    .GetCollection<Author>("authors")
                    .DeleteManyAsync(session, x => x.Id == authorId);

                await client
                    .GetDatabase("Blog")
                    .GetCollection<Blog>("blogs")
                    .DeleteManyAsync(session, x => x.AuthorId == authorId);

//                await session.CommitTransactionAsync();
                session.AbortTransaction();
            }
        }
    }
}