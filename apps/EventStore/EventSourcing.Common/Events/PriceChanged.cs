using System;

namespace EventSourcing.Common.Events
{
    public record PriceChanged
    (
        Guid OrderId,
        decimal OldPrice,
        decimal NewPrice,
        DateTime Date
    );
}