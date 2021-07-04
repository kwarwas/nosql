using System;

namespace EventSourcing.Common.Events
{
    public record OrderSubmitted
    (
        Guid Id,
        string Name,
        decimal Price,
        OrderCategory Category
    );
}