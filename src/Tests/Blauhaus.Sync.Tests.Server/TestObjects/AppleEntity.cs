using System;
using Blauhaus.Sync.Common.Entity;

namespace Blauhaus.Sync.Tests.Server.TestObjects
{
    public class AppleEntity : IEntity
    {
        public Guid Id { get; set; }
        public EntityState EntityState { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public string Colour { get; set; }
    }
}