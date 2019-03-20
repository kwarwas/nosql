using System;
using System.Collections.Generic;

namespace BlogApp.Entities
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Author Author { get; set; }
        public ICollection<Post> Posts { get; private set; } = new List<Post>();

        public void AddPost(Post post)
        {
            Posts.Add(post);
        }
    }
}