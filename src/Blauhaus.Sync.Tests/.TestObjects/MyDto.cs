using System;
using Blauhaus.Domain.Abstractions.Entities;

namespace Blauhaus.Sync.Tests.TestObjects
{
    public class MyDto : IClientEntity<Guid>
    {
        public MyDto()
        {
            Id =  Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public EntityState EntityState { get; set; }
        public long ModifiedAtTicks { get; set; }
        public string Name { get; set; } = null!;
    }
}