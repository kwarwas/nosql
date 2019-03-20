using System;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Database;
using BlogApp.Entities;
using Bogus;
using MongoDB.Driver;

namespace BlogApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new DatabaseContext("Blog");

            await GenerateData(context);
            await Query(context);
            await Replace(context);
            await Delete(context);
        }

        private static Faker<Blog> GenerateFakeBlogData()
        {
            return new Faker<Blog>()
                .RuleFor(x => x.Id, Guid.NewGuid)
                .RuleFor(x => x.Name, faker => faker.Random.Words(5))
                .RuleFor(x => x.Author, faker => new Author(faker.Person.FirstName, faker.Person.LastName))
                .RuleFor(x => x.Posts, faker => new Faker<Post>()
                    .CustomInstantiator(_ => new Post
                    (
                        Guid.NewGuid(),
                        faker.Random.Words(2),
                        faker.Lorem.Paragraph(5),
                        faker.Date.Past()
                    ))
                    .Generate(faker.Random.Number(5))
                );
        }

        private static async Task GenerateData(DatabaseContext context)
        {
            Console.WriteLine("Generating data...");

            var blogs = GenerateFakeBlogData().Generate(20);

            Console.WriteLine("Data generated");
            Console.WriteLine("Saving data...");
            
            await context.GetCollection<Blog>().InsertManyAsync(blogs);
            
            Console.WriteLine("Data saved");
        }

        private static Task Query(DatabaseContext context)
        {
            Console.WriteLine("Querying collection...");

            var posts = context.GetCollection<Blog>()
                .AsQueryable()
                .Where(x => x.Posts.Count > 3)
                .SelectMany(x => x.Posts, (blog, post) => new
                {
                    BLogName = blog.Name,
                    Author = blog.Author.FirstName + " " + blog.Author.LastName,
                    PostTitle = post.Title,
                    PostDate = post.CreatedOn
                })
                .OrderByDescending(x => x.PostDate);

            foreach (var post in posts)
            {
                Console.WriteLine(post);
            }

            Console.WriteLine("Querying finished");

            return Task.CompletedTask;
        }

        private static async Task Replace(DatabaseContext context)
        {
            Console.WriteLine("Replacing data...");

            var postEntity = context.GetCollection<Blog>()
                .AsQueryable()
                .SelectMany(x => x.Posts, (blog, post) => new
                {
                    BlogId = blog.Id,
                    BLogName = blog.Name,
                    Author = blog.Author.FirstName + " " + blog.Author.LastName,
                    PostTitle = post.Title,
                    PostDate = post.CreatedOn
                })
                .First();

            Console.WriteLine(postEntity);

            var newBlogData = GenerateFakeBlogData().Generate();
            
            newBlogData.Id = postEntity.BlogId;

            await context.GetCollection<Blog>()
                .ReplaceOneAsync(x => x.Id == postEntity.BlogId, newBlogData);

            postEntity = context.GetCollection<Blog>()
                .AsQueryable()
                .SelectMany(x => x.Posts, (blog, post) => new
                {
                    BlogId = blog.Id,
                    BLogName = blog.Name,
                    Author = blog.Author.FirstName + " " + blog.Author.LastName,
                    PostTitle = post.Title,
                    PostDate = post.CreatedOn
                })
                .First();

            Console.WriteLine(postEntity);

            Console.WriteLine("Replacing finished");
        }
        
        private static async Task Delete(DatabaseContext context)
        {
            Console.WriteLine("Deleting data...");

            await context.GetCollection<Blog>().DeleteManyAsync(x => true);

            Console.WriteLine("Deleting finished");
        }
    }
}