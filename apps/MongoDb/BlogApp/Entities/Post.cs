using System;

namespace BlogApp.Entities
{
    public class Post
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public Post(Guid id, string title, string body, DateTime createdOn)
        {
            Id = id;
            Title = title;
            Body = body;
            CreatedOn = createdOn;
        }
    }
}