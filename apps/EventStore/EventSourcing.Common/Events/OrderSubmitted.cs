using System;

namespace EventSourcing.Common.Events
{
    public class OrderSubmitted
    {
        public Guid Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public OrderCategory Category { get; }

        public OrderSubmitted(Guid id, string name, decimal price, OrderCategory category)
        {
            Id = id;
            Name = name;
            Price = price;
            Category = category;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Price: {Price}, Category: {Category}";
        }
    }
}