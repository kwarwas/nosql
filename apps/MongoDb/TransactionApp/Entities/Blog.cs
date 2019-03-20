using System;
using System.Collections.Generic;

namespace TransactionApp.Entities
{
    public class Blog
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid AuthorId { get; private set; }

        public Blog(Guid id, string name, Guid authorId)
        {
            Id = id;
            Name = name;
            AuthorId = authorId;
        }
    }
}