using System;

namespace Blauhaus.Sync.Common.Entity
{
    public interface IEntity
    {
        Guid Id { get; set; }
        EntityState EntityState { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset ModifiedAt { get; set; }
    }
}